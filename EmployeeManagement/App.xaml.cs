using EmployeeManagement.Services;
using EmployeeManagement.Services.Interfaces;
using EmployeeManagement.ViewModels;
using EmployeeManagement.Views;
using EmployeeManagement.Views.Dialogs;
using System.Windows;

namespace EmployeeManagement
{
    public partial class App : PrismApplication
    {
        public const string NAME = "Name";
        public const string SURNAME = "Surname";
        public const string AGE = "Age";
        public const string SALARY = "Salary";
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<EditDialog, EditDialogViewModel>();
            containerRegistry.RegisterSingleton<IEmployeeStorageService, FileEmployeeStorageService>();
        }
    }
}
