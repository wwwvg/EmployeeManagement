using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagement.Models;
using EmployeeManagement.Services.Interfaces;
using System.Collections.ObjectModel;

namespace EmployeeManagement.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel(IDialogService dialogService, IEmployeeStorageService storageService)
        {
            //var employee = new Employee { Name = "Vasily", Surname = "Grigoriev", Age = 42, Salary = 300000 };
            //_employees.Add(employee);
            SetEmptyWidth();
            _dialogService = dialogService;
            _storageService = storageService;
        }

        #region PROPERTIES

        IDialogService _dialogService;
        IEmployeeStorageService _storageService;

        [ObservableProperty]
        ObservableCollection<Employee> _employees = new();

        [ObservableProperty]
        Employee _selectedEmployee = null;

        [ObservableProperty]
        bool _isLoading = false;

        [ObservableProperty]
        bool _isProgressBarVisible = true;

        [ObservableProperty]
        string _statusMessage = "";

        [ObservableProperty]
        bool _isInterfaceEnabled = true;

        [ObservableProperty]
        bool _isLoadEmployeesCommandEnabled = true;

        [ObservableProperty]
        bool _isSaveEmployeesCommandEnabled = false;

        [ObservableProperty]
        bool _isCreateEmployeeCommandEnabled = false;

        [ObservableProperty]
        bool _isEditEmployeeCommandEnabled = false;

        [ObservableProperty]
        bool _isDeleteEmployeeCommandEnabled = false;

        [ObservableProperty]
        bool _isAddingEmployee = false;

        [ObservableProperty]
        bool _isAgeVisible = false;

        [ObservableProperty]
        bool _isSalaryVisible = false;

        [ObservableProperty]
        string _ageWidth = "0";

        [ObservableProperty]
        string _salaryWidth = "0";

        [ObservableProperty]
        string _emptyWidth = "0";
        #endregion

        #region METHODS
        [RelayCommand]
        void ChangeAgeVisibility()
        {
            if (IsAgeVisible)
                AgeWidth = "2*";
            else
                AgeWidth = "0";

            SetEmptyWidth();
        }

        [RelayCommand]
        void ChangeSalaryVisibility()
        {
            if (IsSalaryVisible)
                SalaryWidth = "2*";
            else
                SalaryWidth = "0";

            SetEmptyWidth();
        }

        //чтобы ширина первых двух столбцов не менялась при скрытии последних двух столбцов
        void SetEmptyWidth()
        {
            var AgeAndSalaryWidth = (int.Parse(AgeWidth.Replace("*", "")) + int.Parse(SalaryWidth.Replace("*", "")));
            EmptyWidth = (4 - AgeAndSalaryWidth).ToString() + "*";
        }

        [RelayCommand]
        async void LoadEmployees()
        {
            IsLoading = true;
            StatusMessage = "Идет загрузка...";
            try
            {
                if (Employees.Count != 0)
                {
                    Employees.Clear();
                    SetButtonsEnabled(true, true, true, false, false);
                    return;
                }
                await Task.Delay(2000);
                var employees = await _storageService.LoadEmployeesAsync();
                Employees.AddRange(employees);
                StatusMessage = "Загрузка завершена!";
                IsProgressBarVisible = false;
                await Task.Delay(1000);
                IsProgressBarVisible = true;
                SetButtonsEnabled(true, false, true, false, false);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        async void SaveEmployees()
        {
            await _storageService.SaveEmployeesAsync(Employees.ToList<Employee>());
        }

        [RelayCommand]
        void CreateEmployee()
        {
            var dummy = GetDummy();
            var parameters = new DialogParameters { {"Name", dummy.Item1}, {"Surname", dummy.Item2}, { "Age", dummy.Item3}, { "Salary", dummy.Item4} };
            _dialogService.ShowDialog("EditDialog", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    var name = result.Parameters["Name"]?.ToString() ?? string.Empty;
                    var surname = result.Parameters["Surname"]?.ToString() ?? string.Empty;
                    var ageCorrect = int.TryParse(result.Parameters["Age"]?.ToString(), out int age);
                    var salaryCorrect = double.TryParse(result.Parameters["Salary"]?.ToString(), out double salary);
                    if (ageCorrect && salaryCorrect && name != string.Empty && surname != string.Empty)
                    {
                        Employees.Add(new Employee { Name = name, Surname = surname, Age = age, Salary = salary });
                    }
                }
            });
        }

        (string, string, int, double) GetDummy()
        {
            var name = $"Имя{Employees.Count + 1}";
            var surname = $"Фамилия{Employees.Count + 1}";
            var age = new Random().Next(16, 100);
            var salary = Math.Round((decimal)(new Random().Next(160000, 300000)) / 5000) * 5000;
            return new(name, surname, age, (double)salary);
        }

        void SetButtonsEnabled(bool load, bool save, bool add, bool change, bool delete)
        {
            IsLoadEmployeesCommandEnabled = load;
            IsSaveEmployeesCommandEnabled = save;
            IsCreateEmployeeCommandEnabled = add;
            IsEditEmployeeCommandEnabled = change;
            IsDeleteEmployeeCommandEnabled = delete;
        }

        #endregion
    }
}
