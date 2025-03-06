using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            var employee = new Employee { Name = "Vasily", Surname = "Grigoriev", Age = 42, Salary = 300000 };
            _employees.Add(employee);
            SetEmptyWidth();
        }


        [ObservableProperty]
        ObservableCollection<Employee> _employees = new();

        [ObservableProperty]
        Employee _selectedEmployee = null;

        [ObservableProperty]
        bool _isLoading = false;

        [ObservableProperty]
        bool _isInterfaceEnabled = true;

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

        [RelayCommand]
        void ChangeAgeVisibility()
        {
            if(IsAgeVisible)
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
    }
}
