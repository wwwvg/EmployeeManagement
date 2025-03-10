using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace EmployeeManagement.ViewModels
{
    public partial class ConfirmationDialogViewModel : ObservableObject, INavigationAware
    {
        public ConfirmationDialogViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        #region PROPERTIES
        private string _returnViewName;
        IRegionManager _regionManager;

        [ObservableProperty]
        private string? _message;
        #endregion

        #region METHODS
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
        #endregion

        #region РЕАЛИЗАЦИЯ ИНТЕРФЕЙСА НАВИГАЦИИ
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
        #endregion
    }
}
