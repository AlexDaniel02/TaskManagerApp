using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManager.Models;
using TaskManager.ViewModels;
using TaskManager.Views;

namespace TaskManager.Managers
{
    public class ToDoListsManager
    {
        private MainViewModel _mainViewModel;

        public ToDoListsManager(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public bool DeleteSubToDoList(ToDoList toDoList, ToDoList parentToDoList, ToDoList subToDoList)
        {
            if (toDoList.SubToDoLists.Contains(subToDoList))
            {
                toDoList.SubToDoLists.Remove(subToDoList);
                return true;
            }

            foreach (var subList in toDoList.SubToDoLists)
            {
                DeleteSubToDoList(subList, toDoList, subToDoList);
            }
            return false;
        }
        public void MoveDown()
        {
            if (_mainViewModel.ToDoLists.Contains(_mainViewModel.CurrentToDoList))
            {
                var index = _mainViewModel.ToDoLists.IndexOf(_mainViewModel.CurrentToDoList);
                if (index < _mainViewModel.ToDoLists.Count - 1)

                {
                    _mainViewModel.ToDoLists.Move(index, index + 1);
                }
            }
            else
            {
                var parentToDoList = FindParentToDoList(_mainViewModel.CurrentToDoList, _mainViewModel.ToDoLists);
                var index = parentToDoList.SubToDoLists.IndexOf(_mainViewModel.CurrentToDoList);
                if (index < parentToDoList.SubToDoLists.Count - 1)
                {
                    parentToDoList.SubToDoLists.Move(index, index + 1);
                }
            }
        }
        public ToDoList FindParentToDoList(ToDoList childToDoList, IEnumerable<ToDoList> toDoLists)
        {
            foreach (var toDoList in toDoLists)
            {
                if (toDoList.SubToDoLists.Contains(childToDoList))
                {
                    return toDoList;
                }
                else
                {
                    var parentToDoList = FindParentToDoList(childToDoList, toDoList.SubToDoLists);
                    if (parentToDoList != null)
                    {
                        return parentToDoList;
                    }
                }
            }
            return null;
        }
        public void AddToDoList()
        {
            if (_mainViewModel.CurrentToDoList != null)
            {
                var addToDoListViewModel = new AddTDLViewModel(_mainViewModel.ToDoLists, _mainViewModel, _mainViewModel.CurrentToDoList);
                new AddTDLView { DataContext = addToDoListViewModel }.Show();
            }
            else
            {
                var addToDoListViewModel = new AddTDLViewModel(_mainViewModel.ToDoLists, _mainViewModel, null);
                new AddTDLView { DataContext = addToDoListViewModel }.Show();

            }
        }
        public void AddRootToDoList()
        {
            var addToDoListViewModel = new AddRootTDLViewModel(_mainViewModel.ToDoLists, _mainViewModel);
            new AddTDLView { DataContext = addToDoListViewModel }.Show();
        }
        public void MoveUp()
        {
            if (_mainViewModel.ToDoLists.Contains(_mainViewModel.CurrentToDoList))
            {
                var index = _mainViewModel.ToDoLists.IndexOf(_mainViewModel.CurrentToDoList);
                _mainViewModel.ToDoLists.Move(index, index - 1);
            }
            else
            {
                var parentToDoList = FindParentToDoList(_mainViewModel.CurrentToDoList, _mainViewModel.ToDoLists);
                var index = parentToDoList.SubToDoLists.IndexOf(_mainViewModel.CurrentToDoList);
                if (index > 0)
                {
                    parentToDoList.SubToDoLists.Move(index, index - 1);
                }
            }
        }
        public void DeleteToDoList()
        {
            if (_mainViewModel.CurrentToDoList == null)
            {
                return;
            }
            if (_mainViewModel.ToDoLists.Contains(_mainViewModel.CurrentToDoList))
            {
                _mainViewModel.ToDoLists.Remove(_mainViewModel.CurrentToDoList);
            }
            else
            {
                for (int i = 0; i < _mainViewModel.ToDoLists.Count; i++)
                {

                    if (_mainViewModel.toDoListsManager.DeleteSubToDoList(_mainViewModel.ToDoLists[i], _mainViewModel.ToDoLists[i], _mainViewModel.CurrentToDoList))
                    {
                        _mainViewModel.OnPropertyChanged(nameof(_mainViewModel.ToDoLists));
                        return;
                    }
                }
            }
        }
        public void EditToDoList()
        {
            if (_mainViewModel.CurrentToDoList == null)
            {
                MessageBox.Show("You need to select a To Do List first");
            }
            else
            {
                var editTDLViewModel = new EditTDLViewModel(_mainViewModel.CurrentToDoList, _mainViewModel);
                new EditToDoListView { DataContext = editTDLViewModel }.Show();
            }
        }
        public bool IsDuplicateName(string name, ToDoList parentList = null)
        {
            if (parentList == null)
            {
                return _mainViewModel.ToDoLists.Any(tdl => tdl.Title == name) ||
                       _mainViewModel.ToDoLists.Any(tdl => IsDuplicateName(name, tdl));
            }
            else
            {
                return parentList.SubToDoLists.Any(tdl => tdl.Title == name) ||
                       parentList.SubToDoLists.Any(tdl => IsDuplicateName(name, tdl));
            }
        }
    }
}
