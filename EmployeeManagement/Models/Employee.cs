using CommunityToolkit.Mvvm.ComponentModel;

namespace EmployeeManagement.Models
{
    public partial class Employee : ObservableObject
    {
        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _surname;

        [ObservableProperty]
        private int _age;

        [ObservableProperty]
        private double _salary;
    }
}
