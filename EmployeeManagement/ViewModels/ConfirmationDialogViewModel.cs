using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Controls;
using System.Xml.Linq;

namespace EmployeeManagement.ViewModels
{
    public partial class ConfirmationDialogViewModel : ObservableObject, IDialogAware
    {

        [ObservableProperty]
        private string? _message;

        public DialogCloseListener RequestClose { get; }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters[App.MESSAGE]?.ToString() ?? string.Empty;
        }

        [RelayCommand]
        void Ok()
        {
            var result = new DialogResult
            {
                Result = ButtonResult.OK
            };
            RequestClose.Invoke(result);
        }

        [RelayCommand]
        void Cancel()
        {
            RequestClose.Invoke(ButtonResult.Cancel);
        }
    }
}
