using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManager.Commands;
using TaskManager.Managers;

namespace TaskManager.ViewModels
{
    public class FindTaskViewModel : BaseViewModel
    {
        private string _searchText;
        private MainViewModel _mainViewModel;
        private ObservableCollection<Tuple<string, DateTime, string>> _foundTasks;
        public TasksManager tasksManager;
        public ObservableCollection<Tuple<string, DateTime, string>> FoundTasks
        {
            get => _foundTasks;
            set
            {
                _foundTasks = value;
                OnPropertyChanged(nameof(FoundTasks));
            }
        }
        public ICommand FindTaskCommand { get; }
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        
        public FindTaskViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            FindTaskCommand = new RelayCommand(ExecuteSearchCommand);
            tasksManager = new TasksManager(_mainViewModel);
        }
        public FindTaskViewModel()
        {
        }
        private void ExecuteSearchCommand()
        {
            var foundTasks = new ObservableCollection<Tuple<string, DateTime, string>>();
            tasksManager.SearchForTask(_mainViewModel.ToDoLists, SearchText, foundTasks);
            FoundTasks = foundTasks;
            OnPropertyChanged(nameof(FoundTasks));
        }
    }
}
