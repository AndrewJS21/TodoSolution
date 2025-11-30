using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Todo.Core;

public class TodoList
{
    private readonly List<TodoItem> _items = new();

    public IReadOnlyList<TodoItem> Items => _items.AsReadOnly();

    public TodoItem Add(string title)
    {
        var item = new TodoItem(title);
        _items.Add(item);
        return item;
    }

    public bool Remove(Guid id) => _items.RemoveAll(i => i.Id == id) > 0;

    public IEnumerable<TodoItem> Find(string substring) =>
        _items.Where(i => i.Title.Contains(substring ?? string.Empty, StringComparison.OrdinalIgnoreCase));

    public int Count => _items.Count;

    public void Save(string path)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(path, JsonSerializer.Serialize(_items, options));
    }

    public static TodoList Load(string path)
    {
        var list = new TodoList();
        if (!File.Exists(path)) return list;

        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true
        };

        var items = JsonSerializer.Deserialize<List<TodoItem>>(File.ReadAllText(path), options);
        if (items != null)
        {
            list._items.AddRange(items);
        }

        return list;
    }
}