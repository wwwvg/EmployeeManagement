using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagement.Models;

namespace EmployeeManagement.ViewModels
{
    public partial class EditDialogViewModel : ObservableObject, IDialogAware
    {
        public EditDialogViewModel()
        {

        }

        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _surname;

        [ObservableProperty]
        private int _age;

        [ObservableProperty]
        private double _salary;

        [RelayCommand]
        void Ok()
        {
            var result = new DialogResult
            {
                Parameters = new DialogParameters { { App.NAME, Name }, { App.SURNAME, Surname }, { App.AGE, Age }, { App.SALARY, Salary } },
                Result = ButtonResult.OK
            };
            RequestClose.Invoke(result);
        }

        [RelayCommand]
        void Cancel()
        {
            RequestClose.Invoke(ButtonResult.Cancel);
        }

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
            var name = parameters[App.NAME]?.ToString() ?? string.Empty;
            var surname = parameters[App.SURNAME]?.ToString() ?? string.Empty;
            var ageCorrect = int.TryParse(parameters[App.AGE]?.ToString(), out int age);
            var salaryCorrect = int.TryParse(parameters[App.SALARY]?.ToString(), out int salary);
            if (ageCorrect && salaryCorrect && name != string.Empty && surname != string.Empty)
            {
                Name = name;
                Surname = surname;
                Age = age;
                Salary = salary;
            }
        }
    }
}
