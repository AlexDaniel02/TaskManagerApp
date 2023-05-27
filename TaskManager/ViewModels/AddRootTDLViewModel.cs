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
    public class AddRootTDLViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private string _name;
        public ObservableCollection<ToDoList> _toDoLists;
        public ICommand SubmitToDoListCommand { get; }
        public ToDoListsManager toDoListsManager;
        public AddRootTDLViewModel(ObservableCollection<ToDoList> toDoLists, MainViewModel mainViewModel)
        {
            _toDoLists = toDoLists;
            SubmitToDoListCommand = new RelayCommand(ExecuteAddToDoListCommand);
            _mainViewModel = mainViewModel;
            toDoListsManager = new ToDoListsManager(_mainViewModel);
        }
        public AddRootTDLViewModel()
        {

        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
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
                _toDoLists.Add(toDoList);
            }
            _mainViewModel.OnPropertyChanged(nameof(_mainViewModel.ToDoLists));
            currentWindow.Close();
        }
    }
}
