using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TaskManager.Commands;
using TaskManager.Managers;
using TaskManager.Models;
using TaskManager.Views;
using Task = TaskManager.Models.Task;
namespace TaskManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private Statistics _statistics;
        public ICommand AddTaskCommand { get; }
        public ToDoList _currentToDoList;
        public ObservableCollection<ToDoList> ToDoLists { get; set; }
        public ICommand ExitCommand { get; } = new RelayCommand(() => Application.Current.Shutdown());
        public ICommand DeleteToDoListCommand { get; set; }
        public ToDoListsManager toDoListsManager;
        public ICommand DeleteTaskCommand { get; set; }
        private Task _selectedTask;
        public ICommand OpenAboutCommand { get; }
        public ICommand MoveUpCommand { get; set; }
        public ICommand MoveDownCommand { get; set; }
        public ICommand AddToDoListCommand { get; set; }
        public ICommand MoveTaskDownCommand { get; set; }
        public ICommand MoveTaskUpCommand { get; set; }
        public ICommand OpenDatabaseCommand { get; }
        public ICommand ArchiveDatabaseCommand { get; }
        public ICommand NewDatabaseCommand { get; }
        public ICommand EditToDoListCommand { get; }
        public ICommand SetDoneCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand FindTaskCommand { get; }
        public ICommand OpenCategoryManagerCommand { get; }
        public ICommand SortByPriorityCommand { get; }
        public ICommand SortByDeadlineCommand { get; }
        public ICommand StatisticsCommand { get; }
        public ICommand AddRootToDoListCommand { get; }
        public SerializationManager serializationManager;
        TasksManager tasksManager;
        public bool IsSelected = false;
        public Statistics Statistics
        {
            get
            {
                return _statistics;
            }
            set
            {
                _statistics = value;
                OnPropertyChanged(nameof(Statistics));
            }
        }
        public void OpenAbout()
        {
            new AboutView().Show();
        }
        public Task SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }
        public ToDoList CurrentToDoList
        {
            get { return _currentToDoList; }
            set
            {
                if (_currentToDoList != value)
                {
                    _currentToDoList = value;
                    OnPropertyChanged("CurrentToDoList");

                }
            }
        }
        public MainViewModel()
        {
            Statistics = new Statistics();
            toDoListsManager = new ToDoListsManager(this);
            serializationManager = new SerializationManager(this);
            tasksManager = new TasksManager(this);
            OpenDatabaseCommand = new RelayCommand(serializationManager.OpenDatabase);
            AddTaskCommand = new RelayCommand(tasksManager.AddTask);
            ToDoLists = new ObservableCollection<ToDoList>();
            DeleteTaskCommand = new RelayCommand(tasksManager.DeleteTask);
            DeleteToDoListCommand = new RelayCommand(toDoListsManager.DeleteToDoList);
            OpenAboutCommand = new RelayCommand(OpenAbout);
            MoveDownCommand = new RelayCommand(toDoListsManager.MoveDown);
            MoveUpCommand = new RelayCommand(toDoListsManager.MoveUp);
            SetDoneCommand = new RelayCommand(tasksManager.SetDone);
            AddToDoListCommand = new RelayCommand(toDoListsManager.AddToDoList);
            MoveTaskDownCommand = new RelayCommand(tasksManager.MoveTaskDown);
            MoveTaskUpCommand = new RelayCommand(tasksManager.MoveTaskUp);
            NewDatabaseCommand = new RelayCommand(serializationManager.NewDatabase);
            ArchiveDatabaseCommand = new RelayCommand(serializationManager.SerializeToDoLists);
            EditToDoListCommand = new RelayCommand(toDoListsManager.EditToDoList);
            EditTaskCommand = new RelayCommand(tasksManager.EditTask);
            FindTaskCommand = new RelayCommand(tasksManager.FindTask);
            SortByPriorityCommand = new RelayCommand(tasksManager.SortByPriority);
            SortByDeadlineCommand = new RelayCommand(tasksManager.SortByDeadline);
            StatisticsCommand = new RelayCommand(tasksManager.UpdateStatistics);
            AddRootToDoListCommand = new RelayCommand(toDoListsManager.AddRootToDoList);
        }
    }
}
