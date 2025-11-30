using System.Text.Json.Serialization;

namespace Todo.Core;

public class TodoItem
{
    public Guid Id { get; }
    public string Title { get; private set; }
    public bool IsDone { get; private set; }

    // Основной конструктор — для ручного создания
    public TodoItem(string title)
    {
        Id = Guid.NewGuid();
        Title = title?.Trim() ?? throw new ArgumentNullException(nameof(title));
    }

    // Конструктор для десериализации JSON
    [JsonConstructor]
    public TodoItem(Guid id, string title, bool isDone)
    {
        Id = id;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        IsDone = isDone;
    }

    public void MarkDone() => IsDone = true;
    public void MarkUndone() => IsDone = false;

    public void Rename(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Title required", nameof(newTitle));
        Title = newTitle.Trim();
    }
}