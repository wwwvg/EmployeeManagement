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
using EmployeeManagement.Views;
using DryIoc;

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
            try
            {
                if (Employees.Count != 0)
                {
                    Employees.Clear();
                    return;
                }
                IsInterfaceEnabled = false;
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
            }
            finally
            {
                IsDialogShowning = false;
                IsInterfaceEnabled = true;
                SetButtonsEnabled(load: true, save: true, add: true, edit: false, delete: false);
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
            IsInterfaceEnabled = false;
            IsDialogShowning = true;
            var dummy = GetDummy();
            var employee = new Employee { Name = dummy.Name, Surname = dummy.Surname, Age = dummy.Age, Salary = dummy.Salary };
            var parameters = new NavigationParameters();
            parameters.Add("ReturnViewName", nameof(EmployeeData));
            parameters.Add("Employee", employee);
            _regionManager.RequestNavigate("DialogRegion", "EditDialog", parameters);
        }

        [RelayCommand]
        void EditEmployee()
        {
            IsInterfaceEnabled = false;
            IsDialogShowning = true;
            var dummy = GetDummy();
            var employee = new Employee { Name = SelectedEmployee.Name, Surname = SelectedEmployee.Surname, Age = SelectedEmployee.Age, Salary = SelectedEmployee.Salary };
            var parameters = new NavigationParameters();
            parameters.Add("ReturnViewName", nameof(EmployeeData));
            parameters.Add("Employee", employee);
            _regionManager.RequestNavigate("DialogRegion", "EditDialog", parameters);
        }

        [RelayCommand]
        void DeleteEmployee()
        {
            IsInterfaceEnabled = false;
            var parameters = new NavigationParameters();
            parameters.Add("ReturnViewName", nameof(EmployeeData));
            parameters.Add("Message", "Удалить выбранного работника?");
            _regionManager.RequestNavigate("DialogRegion", "ConfirmationDialog", parameters);
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
            if (navigationContext.Parameters.ContainsKey("DeleteEmployee") && SelectedEmployee != null)
            {
                bool deleteEmployee = navigationContext.Parameters.GetValue<bool>("DeleteEmployee");
                if(deleteEmployee)
                    Employees.Remove(SelectedEmployee);
            }
            else if (navigationContext.Parameters.ContainsKey("DialogResult"))
            {
                bool dialogResult = navigationContext.Parameters.GetValue<bool>("DialogResult");

                // Если диалог был подтвержден (кнопка OK)
                if (dialogResult && navigationContext.Parameters.ContainsKey("Result"))
                {
                    var employee = navigationContext.Parameters.GetValue<Employee>("Result");

                    // Если редактировали существующего сотрудника
                    if (SelectedEmployee != null)
                    {
                        // Находим индекс выбранного сотрудника
                        int index = Employees.IndexOf(SelectedEmployee);

                        if (index >= 0)
                        {
                            // Заменяем отредактированного сотрудника в коллекции
                            Employees[index] = employee;
                        }
                    }
                    else
                    {
                        // Добавляем нового сотрудника
                        Employees.Add(employee);
                        SetButtonsEnabled(load: true, save: true, add: true, edit: false, delete: false);
                    }
                }
            }
            // Сбрасываем выбранного сотрудника после завершения операции
            SelectedEmployee = null;
            _regionManager.RequestNavigate("DialogRegion", "EmptyDialog");
            IsInterfaceEnabled = true;
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
