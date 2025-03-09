using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagement.Models;
using EmployeeManagement.Services.Interfaces;
using EmployeeManagement.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EmployeeManagement.ViewModels
{
	public partial class EmployeeDataViewModel : ObservableObject, INavigationAware
	{
        public EmployeeDataViewModel(IDialogService dialogService, IEmployeeStorageService storageService, IRegionManager regionManager)
        {
            SetEmptyWidth();
            _dialogService = dialogService;
            _storageService = storageService;
            _regionManager = regionManager;
        }

        #region PROPERTIES

        IDialogService _dialogService;
        IEmployeeStorageService _storageService;
        IRegionManager _regionManager;

        [ObservableProperty]
        ObservableCollection<Employee> _employees = new();

        [ObservableProperty]
        Employee _selectedEmployee;

        [ObservableProperty]
        bool _isDialogShowning = false;

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
            SetButtonsEnabled(load: false, save: false, add: false, edit: false, delete: false);
            try
            {
                if (Employees.Count != 0)
                {
                    Employees.Clear();
                    SetButtonsEnabled(load: true, save: true, add: true, edit: false, delete: false);
                    return;
                }
                IsDialogShowning = true;
                var parameters = new NavigationParameters();
                parameters.Add("Message", "Идет загрузка...");
                _regionManager.RequestNavigate("DialogRegion", "ProgressDialog", parameters);
                await Task.Delay(1000);
                var employees = await _storageService.LoadEmployeesAsync();
                Employees.AddRange(employees);
                parameters = new NavigationParameters();
                parameters.Add("Message", "Загрузка завершена!");
                _regionManager.RequestNavigate("DialogRegion", "MessageDialog", parameters);
                await Task.Delay(1000);
                _regionManager.RequestNavigate("DialogRegion", "EmptyDialog");
                SetButtonsEnabled(load: true, save: false, add: true, edit: false, delete: false);
            }
            finally
            {
                IsDialogShowning = false;
            }
        }

        [RelayCommand]
        async void SaveEmployees()
        {
            SetButtonsEnabled(load: true, save: false, add: true, edit: IsEditEmployeeCommandEnabled, delete: IsDeleteEmployeeCommandEnabled);
            await _storageService.SaveEmployeesAsync(Employees.ToList<Employee>());
        }

        [RelayCommand]
        void CreateEmployee()
        {
            IsDialogShowning = true;
            var dummy = GetDummy();
            var employee = new Employee { Name = dummy.Name, Surname = dummy.Surname, Age = dummy.Age, Salary = dummy.Salary };
            var parameters = new NavigationParameters();
            parameters.Add("Employee", employee);
            //_regionManager.RequestNavigate("DialogRegion", "EditDialog", parameters);
            _regionManager.RequestNavigate("DialogRegion", new Uri("EditDialog", UriKind.Relative), result =>
            {
                if (result.Success)
                {
                    //var name = result.Context.Parameters[App.NAME]?.ToString() ?? string.Empty;
                    //var surname = result.Parameters[App.SURNAME]?.ToString() ?? string.Empty;
                    //var ageCorrect = int.TryParse(result.Parameters[App.AGE]?.ToString(), out int age);
                    //var salaryCorrect = double.TryParse(result.Parameters[App.SALARY]?.ToString(), out double salary);
                    //if (ageCorrect && salaryCorrect && name != string.Empty && surname != string.Empty)
                    //{
                    //    Employees.Add(new Employee { Name = name, Surname = surname, Age = age, Salary = salary });
                    //}
                    //SetButtonsEnabled(load: true, save: true, add: true, edit: IsEditEmployeeCommandEnabled, delete: IsDeleteEmployeeCommandEnabled);
                }
            }, parameters);
        }

        [RelayCommand]
        void EditEmployee()
        {
            if (SelectedEmployee != null)
            {
                var parameters = new DialogParameters { { App.NAME, SelectedEmployee.Name }, { App.SURNAME, SelectedEmployee.Surname }, { App.AGE, SelectedEmployee.Age }, { App.SALARY, SelectedEmployee.Salary } };
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
                            for (int i = 0; i < Employees.Count; i++)
                            {
                                var editedEmployee = new Employee { Name = name, Surname = surname, Age = age, Salary = salary };
                                if (Object.ReferenceEquals(Employees[i], SelectedEmployee) && editedEmployee != SelectedEmployee)
                                {
                                    Employees[i] = editedEmployee;
                                    SetButtonsEnabled(load: true, save: true, add: true, edit: IsEditEmployeeCommandEnabled, delete: IsDeleteEmployeeCommandEnabled);
                                    return;
                                }
                            }
                        }
                        SetButtonsEnabled(load: true, save: IsSaveEmployeesCommandEnabled, add: true, edit: IsEditEmployeeCommandEnabled, delete: IsDeleteEmployeeCommandEnabled);
                    }
                });
            }
        }

        [RelayCommand]
        void DeleteEmployee()
        {
            if (SelectedEmployee != null && Employees.Count > 0)
            {
                var parameters = new DialogParameters { { App.MESSAGE, "Удалить выбранного работника?" } };
                _dialogService.ShowDialog("ConfirmationDialog", parameters, result =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        Employees.Remove(SelectedEmployee);
                    }
                });
            }
        }

        (string Name, string Surname, int Age, double Salary) GetDummy()
        {
            var nameSurname = NameGenerator.GetRandomNameAndSurname();
            var name = nameSurname.FirstName;
            var surname = nameSurname.LastName;
            var age = new Random().Next(16, 100);
            var salary = Math.Round((decimal)(new Random().Next(160000, 300000)) / 5000) * 5000;

            return new(name, surname, age, (double)salary);
        }

        void SetButtonsEnabled(bool load, bool save, bool add, bool edit, bool delete)
        {
            IsLoadEmployeesCommandEnabled = load;
            IsSaveEmployeesCommandEnabled = save;
            IsCreateEmployeeCommandEnabled = add;
            IsEditEmployeeCommandEnabled = edit;
            IsDeleteEmployeeCommandEnabled = delete;
        }

        partial void OnSelectedEmployeeChanged(Employee value)
        {
            if (value != null)
            {
                SetButtonsEnabled(load: true, save: IsSaveEmployeesCommandEnabled, add: true, edit: true, delete: true);
            }
            else
            {
                SetButtonsEnabled(load: true, save: IsSaveEmployeesCommandEnabled, add: true, edit: false, delete: false);
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

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
