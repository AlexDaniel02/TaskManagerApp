using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TaskManager.Commands;
using TaskManager.Models;
using Task = TaskManager.Models.Task;

namespace TaskManager.ViewModels
{
    public class AddTaskViewModel : BaseViewModel
    {
        private ToDoList _selectedToDoList;
        private string _name;
        private string _description;
        private DateTime _deadline;
        private Category _category;
        private Priority _priority;
        private readonly MainViewModel _mainViewModel;
        public ICommand SubmitTaskCommand { get; }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        public DateTime Deadline
        {
            get => _deadline;
            set
            {
                _deadline = value;
                OnPropertyChanged(nameof(Deadline));
            }
        }
        public Priority SelectedPriority
        {
            get => _priority;
            set
            {
                _priority = value;
                OnPropertyChanged(nameof(SelectedPriority));
            }
        }
        public Category SelectedCategory
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }
        
        public AddTaskViewModel()
        {
        }
        public AddTaskViewModel(ToDoList selectedToDoList, MainViewModel mainWindowViewModel)
        {
            _selectedToDoList = selectedToDoList;
            _mainViewModel = mainWindowViewModel;
            SubmitTaskCommand = new RelayCommand(ExecuteSubmitTaskCommand);
            Deadline = DateTime.Today;
            SelectedPriority = Priority.Low;
        }
        private void ExecuteSubmitTaskCommand()
        {
            if (Name == null || Description == null)
            {
                MessageBox.Show("Please fill in all fields");

            }
            else
            {
                var task = new Task
                {
                    Name = Name,
                    Description = Description,
                    Deadline = Deadline,
                    Category = SelectedCategory,
                    Priority = SelectedPriority,
                    Status = Status.Created
                };

                _selectedToDoList.Tasks.Add(task);
                MessageBox.Show("Task added successfully.");
            }
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            currentWindow.Close();
        }
    }
}
