using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class ToDoList : BaseModel
    {
        private string _title;
        private ObservableCollection<Task> _tasks;
        public ObservableCollection<ToDoList> SubToDoLists { get; set; }
        public string Title
        {
            get { return _title; }
            set
            {

                _title = value;
                OnPropertyChanged(nameof(Title));

            }
        }
        public ObservableCollection<Task> Tasks
        {
            get { return _tasks; }
            set
            {

                _tasks = value;
                OnPropertyChanged(nameof(Tasks));

            }
        }

        public ToDoList()
        {
            Tasks = new ObservableCollection<Task>();
            SubToDoLists = new ObservableCollection<ToDoList>();
        }
        public ToDoList(string title)
        {
            Tasks = new ObservableCollection<Task>();
            SubToDoLists = new ObservableCollection<ToDoList>();
            Title = title;
        }
    }
}
