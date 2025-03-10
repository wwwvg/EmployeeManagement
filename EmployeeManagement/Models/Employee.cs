﻿using CommunityToolkit.Mvvm.ComponentModel;

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


        public override bool Equals(object? obj)
        {
            if (obj is not Employee other)
                return false;

            return Name == other.Name &&
                   Surname == other.Surname &&
                   Age == other.Age &&
                   Salary == other.Salary;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Surname, Age, Salary);
        }

        public static bool operator ==(Employee? left, Employee? right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Employee? left, Employee? right)
        {
            return !(left == right);
        }
    }
}
