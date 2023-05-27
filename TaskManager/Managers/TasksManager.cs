using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManager.Models;
using TaskManager.ViewModels;
using TaskManager.Views;
using Task = TaskManager.Models.Task;

namespace TaskManager.Managers
{
    public class TasksManager
    {
        private MainViewModel _mainViewModel;
        private bool _isPrioritySortAscending;
        private bool _isDeadlineSortAscending;
        public bool IsPrioritySortAscending
        {
            get { return _isPrioritySortAscending; }
            set { _isPrioritySortAscending = value; _mainViewModel.OnPropertyChanged(); }
        }
        public bool IsDeadlineSortAscending
        {
            get { return _isDeadlineSortAscending; }
            set { _isDeadlineSortAscending = value; _mainViewModel.OnPropertyChanged(); }
        }

        public TasksManager(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _isPrioritySortAscending = true;
            _isDeadlineSortAscending = true;
        }
        public void AddTask()
        {
            var addTaskView = new AddTaskView();
            if (_mainViewModel.CurrentToDoList == null)
            {
                MessageBox.Show("You need to select a To Do List first");
            }
            else
            {
                var addTaskViewModel = new AddTaskViewModel(_mainViewModel.CurrentToDoList, _mainViewModel);
                addTaskView.DataContext = addTaskViewModel;
                new AddTaskView { DataContext = addTaskViewModel }.Show();
            }
        }
        public void SortByPriority()
        {
            if (_mainViewModel.CurrentToDoList != null)
            {
                if (IsPrioritySortAscending)
                {
                    _mainViewModel.CurrentToDoList.Tasks = new ObservableCollection<Task>(_mainViewModel.CurrentToDoList.Tasks.OrderBy(task => task.Priority));
                }
                else
                {
                    _mainViewModel.CurrentToDoList.Tasks = new ObservableCollection<Task>(_mainViewModel.CurrentToDoList.Tasks.OrderByDescending(task => task.Priority));
                }
                IsPrioritySortAscending = !IsPrioritySortAscending;
            }
            else
            {
                MessageBox.Show("You need to select a To Do List first");
            }
        }
        public void SortByDeadline()
        {
            if (_mainViewModel.CurrentToDoList != null)
            {
                if (IsDeadlineSortAscending)
                {
                    _mainViewModel.CurrentToDoList.Tasks = new ObservableCollection<Task>(_mainViewModel.CurrentToDoList.Tasks.OrderBy(task => task.Deadline));
                }
                else
                {
                    _mainViewModel.CurrentToDoList.Tasks = new ObservableCollection<Task>(_mainViewModel.CurrentToDoList.Tasks.OrderByDescending(task => task.Deadline));
                }
                IsDeadlineSortAscending = !IsDeadlineSortAscending;
            }
            else
            {
                MessageBox.Show("You need to select a To Do List first");
            }
        }
        public bool DeleteTask(ToDoList toDoList, ToDoList parentToDoList, Models.Task task)
        {
            if (toDoList.Tasks.Contains(task))
            {
                toDoList.Tasks.Remove(task);
                return true;
            }
            foreach (var subList in toDoList.SubToDoLists)
            {
                DeleteTask(subList, toDoList, task);
            }
            return false;
        }
        public void DeleteTask()
        {
            if (_mainViewModel.SelectedTask != null)
            {
                for (int i = 0; i < _mainViewModel.ToDoLists.Count; i++)
                {
                    if (DeleteTask(_mainViewModel.ToDoLists[i], _mainViewModel.ToDoLists[i], _mainViewModel.SelectedTask))
                    {
                        _mainViewModel.OnPropertyChanged(nameof(_mainViewModel.ToDoLists));
                        return;
                    }
                }
            }
        }
        public void MoveTaskUp()
        {
            if (_mainViewModel.SelectedTask != null)
            {
                var index = _mainViewModel.CurrentToDoList.Tasks.IndexOf(_mainViewModel.SelectedTask);
                if (index > 0)
                {
                    _mainViewModel.CurrentToDoList.Tasks.Move(index, index - 1);
                }
            }
        }
        public void MoveTaskDown()
        {
            if (_mainViewModel.SelectedTask != null)
            {
                var index = _mainViewModel.CurrentToDoList.Tasks.IndexOf(_mainViewModel.SelectedTask);
                if (index < _mainViewModel.CurrentToDoList.Tasks.Count - 1)
                {
                    _mainViewModel.CurrentToDoList.Tasks.Move(index, index + 1);
                }
            }
        }
        public void SetDone()
        {
            if (_mainViewModel.SelectedTask == null)
            {
                MessageBox.Show("You need to select a task first.");
            }
            else
            {
                _mainViewModel.SelectedTask.Status = Status.Done;
                _mainViewModel.SelectedTask.DateFinished = DateTime.Now;
                _mainViewModel.OnPropertyChanged(nameof(_mainViewModel.SelectedTask));
                _mainViewModel.OnPropertyChanged(nameof(_mainViewModel.SelectedTask.DateFinished));
            }
        }
        public void EditTask()
        {
            if (_mainViewModel.SelectedTask == null)
            {
                MessageBox.Show("You need to select a Task first");
            }
            else
            {
                var editTaskViewModel = new EditTaskViewModel(_mainViewModel.SelectedTask, _mainViewModel);
                new EditTaskView { DataContext = editTaskViewModel }.Show();
            }
        }
        public void FindTask()
        {
            var findTaskViewModel = new FindTaskViewModel(_mainViewModel);
            new FindTaskView { DataContext = findTaskViewModel }.Show();
        }
        public void UpdateStatistics()
        {
            _mainViewModel.Statistics.TasksDueToday = 0;
            _mainViewModel.Statistics.TasksDueTomorrow = 0;
            _mainViewModel.Statistics.TasksOverdue = 0;
            _mainViewModel.Statistics.TasksDone = 0;
            CalculateStatistics(_mainViewModel.ToDoLists);
            _mainViewModel.OnPropertyChanged(nameof(Statistics));
        }
        private void CalculateStatistics(IEnumerable<ToDoList> toDoLists)
        {
            foreach (var list in toDoLists)
            {
                foreach (var task in list.Tasks)
                {
                    if (task.Status == Status.Done)
                    {
                        _mainViewModel.Statistics.TasksDone++;
                    }
                    else
                    {
                        var deadline = task.Deadline.Date;
                        var timeNow = DateTime.Now;

                        if (deadline.Day == timeNow.Day)
                        {
                            _mainViewModel.Statistics.TasksDueToday++;
                        }
                        else if (deadline.Day == timeNow.Day + 1)
                        {
                            _mainViewModel.Statistics.TasksDueTomorrow++;
                        }
                        else if (deadline.Day < timeNow.Day)
                        {
                            _mainViewModel.Statistics.TasksOverdue++;
                        }
                    }

                }
                CalculateStatistics(list.SubToDoLists);
            }
        }
        public void SearchForTask(IEnumerable<ToDoList> lists, string searchText, ObservableCollection<Tuple<string, DateTime, string>> foundTasks)
        {
            foreach (var list in lists)
            {
                Debug.WriteLine(lists.Count());
                if (list.Tasks.Any(task => task.Name == searchText))
                {
                    foreach (var task in list.Tasks.Where(task => task.Name == searchText))
                    {
                        foundTasks.Add(new Tuple<string, DateTime, string>(task.Name, task.Deadline, list.Title));
                    }
                }

                SearchForTask(list.SubToDoLists, searchText, foundTasks);
            }
        }
    }
}
