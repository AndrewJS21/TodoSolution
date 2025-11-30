using Xunit;
using Todo.Core;
using System.Linq;
using System.IO;

namespace Todo.Core.Tests;

public class TodoListTests
{
    [Fact]
    public void Add_IncreasesCount()
    {
        var list = new TodoList();
        list.Add("  task ");
        Assert.Equal(1, list.Count);
        Assert.Equal("task", list.Items.First().Title);
    }

    [Fact]
    public void Remove_ById_Works()
    {
        var list = new TodoList();
        var item = list.Add("a");
        Assert.True(list.Remove(item.Id));
        Assert.Equal(0, list.Count);
    }

    [Fact]
    public void Find_ReturnsMatches()
    {
        var list = new TodoList();
        list.Add("Buy milk");
        list.Add("Read book");
        var found = list.Find("buy").ToList();
        Assert.Single(found);
        Assert.Equal("Buy milk", found[0].Title);
    }

    [Fact]
    public void SaveAndLoad_PersistsData()
    {
        var list = new TodoList();
        list.Add("Task 1");
        list.Add("Task 2");

        var tempFile = Path.GetTempFileName();
        try
        {
            list.Save(tempFile);
            var loaded = TodoList.Load(tempFile);
            Assert.Equal(2, loaded.Count);
            Assert.Contains(loaded.Items, i => i.Title == "Task 1");
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void SaveAndLoad_PersistsAllData()
    {
        // Arrange
        var originalList = new TodoList();
        var task1 = originalList.Add("Buy milk");
        task1.MarkDone();
        originalList.Add("Read book");

        var tempFile = Path.GetTempFileName();

        try
        {
            // Act
            originalList.Save(tempFile);
            var loadedList = TodoList.Load(tempFile);

            // Assert
            Assert.Equal(2, loadedList.Count);

            var loadedTasks = loadedList.Items.ToList();
            Assert.Equal("Buy milk", loadedTasks[0].Title);
            Assert.True(loadedTasks[0].IsDone);
            Assert.Equal("Read book", loadedTasks[1].Title);
            Assert.False(loadedTasks[1].IsDone);

            Assert.Equal(task1.Id, loadedTasks[0].Id);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void Load_NonExistentFile_ReturnsEmptyList()
    {
        var list = TodoList.Load("nonexistent-file.json");
        Assert.Empty(list.Items);
    }
}