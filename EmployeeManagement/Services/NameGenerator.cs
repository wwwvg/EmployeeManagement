using System.IO;
using System.Text.Json;

namespace EmployeeManagement.Services
{
    public class NameGenerator
    {
        /// <summary>
        /// Возвращает случайное имя и фамилию из предоставленных списков.
        /// Если списки отсутствуют, возвращает значения по умолчанию "Имя" и "Фамилия".
        /// </summary>
        /// <param name="namesFilePath">Путь к JSON файлу с именами</param>
        /// <param name="surnamesFilePath">Путь к JSON файлу с фамилиями</param>
        /// <returns>Кортеж, содержащий имя и фамилию</returns>
        public static (string FirstName, string LastName) GetRandomNameAndSurname(string namesFilePath = "Resources\\russianNames.json", string surnamesFilePath = "Resources\\russianSurnames.json")
        {
            var random = new Random();
            string firstName = "Имя";
            string lastName = "Фамилия";

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            namesFilePath = Path.Combine(basePath, namesFilePath);
            surnamesFilePath = Path.Combine(basePath, surnamesFilePath);
            try
            {
                // Получаем имя из списка, если он существует
                if (!string.IsNullOrEmpty(namesFilePath) && File.Exists(namesFilePath))
                {
                    var namesJson = File.ReadAllText(namesFilePath);
                    var namesData = JsonSerializer.Deserialize<NamesData>(namesJson);

                    if (namesData != null && namesData.RussianNames != null && namesData.RussianNames.Count > 0)
                    {
                        firstName = namesData.RussianNames[random.Next(namesData.RussianNames.Count)];
                    }
                }

                // Получаем фамилию из списка, если он существует
                if (!string.IsNullOrEmpty(surnamesFilePath) && File.Exists(surnamesFilePath))
                {
                    var surnamesJson = File.ReadAllText(surnamesFilePath);
                    var surnamesData = JsonSerializer.Deserialize<SurnamesData>(surnamesJson);

                    if (surnamesData != null && surnamesData.RussianSurnames != null && surnamesData.RussianSurnames.Count > 0)
                    {
                        lastName = surnamesData.RussianSurnames[random.Next(surnamesData.RussianSurnames.Count)];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при чтении файлов: {ex.Message}");
                // В случае ошибки возвращаем значения по умолчанию
            }

            return (firstName, lastName);
        }

        // Классы для десериализации JSON
        private class NamesData
        {
            public List<string> RussianNames { get; set; }
        }

        private class SurnamesData
        {
            public List<string> RussianSurnames { get; set; }
        }
    }
}
