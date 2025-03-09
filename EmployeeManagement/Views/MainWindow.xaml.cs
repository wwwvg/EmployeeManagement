using System.Windows;

namespace EmployeeManagement.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(EmployeeData));
        }
    }
}