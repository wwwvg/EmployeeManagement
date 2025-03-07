using EmployeeManagement.Models;

namespace EmployeeManagement.Services.Interfaces
{
    public interface IEmployeeStorageService
    {
        Task<List<Employee>> LoadEmployeesAsync();
        Task SaveEmployeesAsync(List<Employee> employees);
    }
}
