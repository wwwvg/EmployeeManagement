using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.ViewModels
{
	public partial class ProgressDialogViewModel : ObservableObject, INavigationAware
	{
        public ProgressDialogViewModel()
        {

        }

        [ObservableProperty]
        string _statusMessage = "";

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
                StatusMessage = navigationContext.Parameters.GetValue<string>("Message");
        }
    }
}
