using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagement.Models;
using System.Windows.Controls;
using System.Xml.Linq;

namespace EmployeeManagement.ViewModels
{
    public partial class ConfirmationDialogViewModel : ObservableObject, INavigationAware
    {
        public ConfirmationDialogViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private string _returnViewName;
        IRegionManager _regionManager;

        [ObservableProperty]
        private string? _message;

        [RelayCommand]
        void Ok()
        {
            // Создаем навигационные параметры
            var parameters = new NavigationParameters();
            parameters.Add("DeleteEmployee", true);

            // Навигация обратно с передачей параметров
            _regionManager.RequestNavigate("ContentRegion", _returnViewName, parameters);
        }

        [RelayCommand]
        void Cancel()
        {
            var parameters = new NavigationParameters();
            parameters.Add("DeleteEmployee", false);
            _regionManager.RequestNavigate("ContentRegion", _returnViewName, parameters);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("ReturnViewName"))
                _returnViewName = navigationContext.Parameters.GetValue<string>("ReturnViewName");
            if (navigationContext.Parameters.ContainsKey("Message"))
                Message = navigationContext.Parameters.GetValue<string>("Message");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
