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

namespace TaskManager.ViewModels
{
    public class AddTDLViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private string _name;
        public ObservableCollection<ToDoList> _toDoLists;
        public ToDoList _parentToDoList;
        public ICommand SubmitToDoListCommand { get; }
        public ToDoListsManager toDoListsManager;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public AddTDLViewModel(ObservableCollection<ToDoList> toDoLists, MainViewModel mainViewModel, ToDoList parentToDoList = null)
        {
            _toDoLists = toDoLists;
            _parentToDoList = parentToDoList;
            SubmitToDoListCommand = new RelayCommand(ExecuteAddToDoListCommand);
            _mainViewModel = mainViewModel;
            toDoListsManager = new ToDoListsManager(_mainViewModel);
        }
        public AddTDLViewModel()
        {
        }
        private void ExecuteAddToDoListCommand()
        {
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            var toDoList = new ToDoList
            {
                Title = Name
            };
            if (toDoListsManager.IsDuplicateName(toDoList.Title))
            {
                MessageBox.Show("This name already exists!");

            }
            else
            {
                if (_parentToDoList == null)
                {
                    _toDoLists.Add(toDoList);
                }
                else
                {
                    _parentToDoList.SubToDoLists.Add(toDoList);

                }
            }
            _mainViewModel.OnPropertyChanged(nameof(_mainViewModel.ToDoLists));
            currentWindow.Close();
        }
    }
}
