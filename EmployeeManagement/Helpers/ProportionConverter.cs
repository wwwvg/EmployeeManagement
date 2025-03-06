using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EmployeeManagement.Helpers
{
    public class ProportionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Первый параметр - ширина ListView
            if (values.Length == 0 || values[0] == DependencyProperty.UnsetValue)
                return 0.0;

            double actualWidth = (double)values[0];

            // Получаем пропорцию из параметра
            double proportion = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);

            // Вычисляем ширину колонки
            return actualWidth * proportion;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
