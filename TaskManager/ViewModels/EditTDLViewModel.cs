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
    public class EditTDLViewModel : BaseViewModel   
    {
        private readonly MainViewModel _mainViewModel;
        private ToDoList _currentToDoList;
        private string _name;
        public ToDoListsManager toDoListsManager;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public ICommand EditTDLCommand { get; set; }
        public EditTDLViewModel(ToDoList currentToDoList, MainViewModel mainViewModel)
        {
            _currentToDoList = currentToDoList;
            EditTDLCommand = new RelayCommand(EditTDL);
            _mainViewModel = mainViewModel;
            toDoListsManager = new ToDoListsManager(_mainViewModel);
        }
        public EditTDLViewModel()
        {
        }
        public void EditTDL()
        {
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            var title = Name;

            if (toDoListsManager.IsDuplicateName(title) || title == null)
            {
                MessageBox.Show("This name is not valid.");
            }
            else
            {
                _currentToDoList.Title = Name;
            }
            currentWindow.Close();
        }
    }
}
