using CommunityToolkit.Mvvm.ComponentModel;

namespace EmployeeManagement.ViewModels
{
    public partial class ProgressDialogViewModel : ObservableObject, INavigationAware
    {
        public ProgressDialogViewModel()
        {

        }

        [ObservableProperty]
        string _message = "";

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
    }
}
