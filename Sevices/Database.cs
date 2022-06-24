using RSS_Avalonia.Models;
using System.Text.Json;

namespace RSS_Avalonia.Services
{
    public class Database
    {
        public IEnumerable<TodoItem> Posts()
        {
            FileStream fs = new FileStream(Environment.CurrentDirectory + "/feeds.json", FileMode.OpenOrCreate);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            
            var items = JsonSerializer.Deserialize<List<TodoItem>>(File.ReadAllText(Environment.CurrentDirectory + "/feeds.json"), options);
            
            return items.OrderByDescending(n => n.Data);
        }
    }
}