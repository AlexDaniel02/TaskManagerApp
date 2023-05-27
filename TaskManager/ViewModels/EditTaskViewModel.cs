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
    public class EditTaskViewModel : BaseViewModel
    {
        private Task _selectedTask;
        private string _name;
        private string _description;
        private DateTime _deadline;
        private Category _category;
        private Priority _priority;
        private Status _status;
        private readonly MainViewModel _mainWindowViewModel;
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
        public Category SelectedCategory
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        public Status SelectedStatus
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
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

        public EditTaskViewModel(Task selectedTask, MainViewModel mainWindowViewModel)
        {
            _selectedTask = selectedTask;
            _mainWindowViewModel = mainWindowViewModel;
            SubmitTaskCommand = new RelayCommand(ExecuteSubmitTaskCommand);
        }
        public EditTaskViewModel()
        {
        }
        private void ExecuteSubmitTaskCommand()
        {
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (Name == null || Description == null)
            {
                MessageBox.Show("Please fill all the fields");

            }
            else
            {
                _selectedTask.Name = Name;
                _selectedTask.Description = Description;
                _selectedTask.Deadline = Deadline;
                _selectedTask.Category = SelectedCategory;
                _selectedTask.Priority = SelectedPriority;
                _selectedTask.Status = SelectedStatus;
            }           currentWindow.Close();
        }
    }
}
