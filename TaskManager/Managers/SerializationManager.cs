using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Managers
{
    public class SerializationManager
    {
        private MainViewModel _mainViewModel;
        public SerializationManager(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public void SerializeToDoLists()
        {
            string fileName = $"ToDoLists_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xml";
            var serializer = new XmlSerializer(typeof(ObservableCollection<ToDoList>));
            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, _mainViewModel.ToDoLists);
            }
        }
        public void OpenDatabase()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                Title = "Open Database"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var xml = File.ReadAllText(openFileDialog.FileName);
                _mainViewModel.ToDoLists = DeserializeFromXml<ObservableCollection<ToDoList>>(xml);
                _mainViewModel.CurrentToDoList = _mainViewModel.ToDoLists.FirstOrDefault();
                _mainViewModel.OnPropertyChanged(nameof(_mainViewModel.ToDoLists));
            }
        }
        public void NewDatabase()
        {
            var todoLists = new ObservableCollection<ToDoList>();
            _mainViewModel.CurrentToDoList = null;
            _mainViewModel.SelectedTask = null;
            string fileName = "ToDoLists_" + DateTime.Now.ToString("yyyyMMdd") + ".xml";
            SerializeToDoLists();
            _mainViewModel.ToDoLists = todoLists;
            _mainViewModel.OnPropertyChanged(nameof(_mainViewModel.ToDoLists));
        }
        public T DeserializeFromXml<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
