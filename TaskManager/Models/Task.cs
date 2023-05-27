using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class Task : BaseModel
    {
        private string _name;
        private DateTime _deadline;
        private DateTime _dateFinished;
        private string _description;
        private Status _status;
        private Priority _priority;
        private Category _category;
        public Category Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {

                _name = value;
                OnPropertyChanged(nameof(Name));

            }
        }
        public DateTime Deadline
        {
            get { return _deadline; }
            set
            {
                _deadline = value;
                OnPropertyChanged(nameof(Deadline));
            }
        }
        public DateTime DateFinished
        {
            get { return _dateFinished; }
            set
            {
                _dateFinished = value;
                OnPropertyChanged(nameof(DateFinished));
            }
        }
        public string FormattedDeadline
        {
            get { return _deadline.ToString("dd/MM/yyyy"); }
        }
        public string FormattedDateFinished
        {
            get
            {
                if (_dateFinished != default)
                {
                    return _dateFinished.ToString("dd/MM/yyyy");
                }
                else
                {
                    return "Not Finished";
                }
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {

                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        public Status Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public Priority Priority
        {
            get { return _priority; }
            set
            {
                _priority = value;
                OnPropertyChanged(nameof(Priority));
            }
        }
        public Task()
        {
            Status = Status.Created;
            Priority = Priority.Low;

        }

        public Task(string name, DateTime deadline, string description, Status status, Priority priority, Category category)
        {
            this.Name = name;
            this.Deadline = deadline;
            this.Description = description;
            this.Status = status;
            this.Priority = priority;
            this.Category = category;
            this.DateFinished = default;
        }
    }
    public enum Status
    {
        Created,
        InProgress,
        Done
    }
    public enum Priority
    {
        Low,
        Medium,
        High
    }
    public enum Category
    {
        Shopping,
        Work,
        School,
        Home,
        Other
    }
}
