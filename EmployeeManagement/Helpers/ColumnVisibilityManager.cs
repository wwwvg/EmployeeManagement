using System.Windows.Controls;

public class ColumnVisibilityManager
{
    private ListView _listView;
    private List<GridViewColumn> _allColumns = new List<GridViewColumn>();
    private Dictionary<int, bool> _columnVisibility = new Dictionary<int, bool>();

    public ColumnVisibilityManager(ListView listView)
    {
        _listView = listView;
        var gridView = (GridView)_listView.View;

        // Сохраняем исходные колонки
        for (int i = 0; i < gridView.Columns.Count; i++)
        {
            _allColumns.Add(gridView.Columns[i]);
            _columnVisibility[i] = true; // По умолчанию все колонки видимы
        }
    }

    public void UpdateColumnVisibility(int columnIndex, bool isVisible)
    {
        if (columnIndex < 0 || columnIndex >= _allColumns.Count)
            throw new ArgumentOutOfRangeException(nameof(columnIndex));

        // Обновляем состояние видимости
        _columnVisibility[columnIndex] = isVisible;

        // Обновляем колонки в GridView
        RefreshGridView();
    }

    private void RefreshGridView()
    {
        var gridView = (GridView)_listView.View;
        gridView.Columns.Clear();

        for (int i = 0; i < _allColumns.Count; i++)
        {
            if (_columnVisibility[i])
            {
                gridView.Columns.Add(_allColumns[i]);
            }
        }
    }

    // Метод для одновременного обновления нескольких колонок
    public void UpdateMultipleColumns(Dictionary<int, bool> columnsVisibility)
    {
        foreach (var pair in columnsVisibility)
        {
            if (pair.Key >= 0 && pair.Key < _allColumns.Count)
            {
                _columnVisibility[pair.Key] = pair.Value;
            }
        }

        RefreshGridView();
    }
}