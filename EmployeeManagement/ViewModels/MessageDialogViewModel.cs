using CommunityToolkit.Mvvm.ComponentModel;

namespace EmployeeManagement.ViewModels
{
    public partial class MessageDialogViewModel : ObservableObject, INavigationAware
    {

        #region PROPERTIES
        [ObservableProperty]
        string _message = "";
        #endregion

        #region РЕАЛИЗАЦИЯ ИНТЕРФЕЙСА НАВИГАЦИИ
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("Message"))
                Message = navigationContext.Parameters.GetValue<string>("Message");
        }
        #endregion
    }
}
