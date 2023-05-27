using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Views
{
    public partial class TaskManagerView : UserControl
    {
        public TaskManagerView()
        {
            InitializeComponent();
        }
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedTdl = e.NewValue as ToDoList;
            if (selectedTdl != null)
            {
                var mainViewModel = DataContext as MainViewModel;
                if (mainViewModel != null)
                {
                    mainViewModel.CurrentToDoList = selectedTdl;
                }
            }

        }
    }
}
