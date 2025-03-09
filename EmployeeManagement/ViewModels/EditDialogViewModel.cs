using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagement.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public partial class EditDialogViewModel : ObservableValidator, IDialogAware
    {
        public EditDialogViewModel()
        {
            // Подписываемся на изменения свойств, чтобы обновлять состояние команды
            PropertyChanged += (s, e) => OkCommand.NotifyCanExecuteChanged();
        }

        public string Title => "Добавление/изменение сотрудника";

        // Поля с валидацией
        [ObservableProperty]
        [Required(ErrorMessage = "Имя не может быть пустым.")]
        [MinLength(1, ErrorMessage = "Имя должно содержать хотя бы 1 символ.")]
        [CustomValidation(typeof(EditDialogViewModel), nameof(ValidateName))]
        private string _name;

        [ObservableProperty]
        [Required(ErrorMessage = "Фамилия не может быть пустой.")]
        [MinLength(1, ErrorMessage = "Фамилия должна содержать хотя бы 1 символ.")]
        [CustomValidation(typeof(EditDialogViewModel), nameof(ValidateSurname))]
        private string _surname;

        [ObservableProperty]
        [Range(1, int.MaxValue, ErrorMessage = "Возраст должен быть больше 0.")]
        private int _age;

        [ObservableProperty]
        [Range(0.01, double.MaxValue, ErrorMessage = "Зарплата должна быть больше 0.")]
        private decimal _salary;

        [RelayCommand(CanExecute = nameof(CanOk))]
        void Ok()
        {
            var result = new DialogResult
            {
                Parameters = new DialogParameters { { App.NAME, Name }, { App.SURNAME, Surname }, { App.AGE, Age }, { App.SALARY, Salary } },
                Result = ButtonResult.OK
            };
            RequestClose.Invoke(result);
        }
        private bool CanOk()
        {
            ValidateAllProperties();
            return !HasErrors;
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
        #region VALIDATION
        // Кастомная валидация для имени
        public static ValidationResult ValidateName(string name, ValidationContext context)
        {
            if (name != null && !name.All(char.IsLetter))
                return new ValidationResult("Имя должно содержать только буквы.");
            return ValidationResult.Success;
        }

        // Кастомная валидация для фамилии
        public static ValidationResult ValidateSurname(string surname, ValidationContext context)
        {
            if (surname != null && !surname.All(char.IsLetter))
                return new ValidationResult("Фамилия должна содержать только буквы.");
            return ValidationResult.Success;
        }

        #endregion
    }  
}
