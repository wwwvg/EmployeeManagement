using EmployeeManagement.Helpers;
using EmployeeManagement.ViewModels;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmployeeManagement.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //_columnManager = new ColumnVisibilityManager(myListView);

            //var viewModel = (MainWindowViewModel)DataContext;
            //viewModel.PropertyChanged += ViewModel_PropertyChanged;

            //// Инициализируем начальное состояние
            //_columnManager.UpdateColumnVisibility(2, viewModel.IsAgeVisible);
            //_columnManager.UpdateColumnVisibility(3, viewModel.IsSalaryVisible);
        }

        //ColumnVisibilityManager _columnManager;
        //private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    var viewModel = (MainWindowViewModel)DataContext;

        //    if (e.PropertyName == nameof(MainWindowViewModel.IsAgeVisible))
        //    {
        //        _columnManager.UpdateColumnVisibility(2, viewModel.IsAgeVisible);
        //    }
        //    else if (e.PropertyName == nameof(MainWindowViewModel.IsSalaryVisible))
        //    {
        //        _columnManager.UpdateColumnVisibility(3, viewModel.IsSalaryVisible);
        //    }
        //}
    }
}