using EmployeeManagement.Models;
using EmployeeManagement.Services.Interfaces;
using System.IO;
using System.Text.Json;

namespace EmployeeManagement.Services
{
    public class FileEmployeeStorageService : IEmployeeStorageService
    {
        private readonly string _filePath;

        public FileEmployeeStorageService(string filePath = "employees.json")
        {
            _filePath = filePath;
        }

        public async Task<List<Employee>> LoadEmployeesAsync()
        {
            if (!File.Exists(_filePath))
                return new List<Employee>();

            using var stream = File.OpenRead(_filePath);
            return await JsonSerializer.DeserializeAsync<List<Employee>>(stream) ?? new List<Employee>();
        }

        public async Task SaveEmployeesAsync(List<Employee> employees)
        {
            using var stream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(stream, employees, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
    }
}
