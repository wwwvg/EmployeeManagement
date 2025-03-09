using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagement.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public partial class EditDialogViewModel : ObservableValidator, INavigationAware
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
        private double _salary;

        [RelayCommand(CanExecute = nameof(CanOk))]
        void Ok()
        {

        }
        private bool CanOk()
        {
            ValidateAllProperties();
            return !HasErrors;
        }

        [RelayCommand]
        void Cancel()
        {

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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("Employee"))
            {
                var employee = navigationContext.Parameters.GetValue<Employee>("Employee");
                Name = employee.Name;
                Surname = employee.Surname;
                Age = employee.Age;
                Salary = employee.Salary;
            }
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
