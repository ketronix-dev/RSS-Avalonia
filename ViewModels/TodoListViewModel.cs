using System.Collections.ObjectModel;
using System.Text.Json;
using Feed;
using OpmlParser;
using RSS_Avalonia.Models;

namespace RSS_Avalonia.ViewModels
{
    public class TodoListViewModel : ViewModelBase
    {
        public TodoListViewModel(IEnumerable<TodoItem> items)
        {
            Items = new ObservableCollection<TodoItem>(items);
        }
        
        public static IEnumerable<TodoItem> Refresh()
        {
            FileStream fs = new FileStream(Environment.CurrentDirectory + "/feeds.json", FileMode.OpenOrCreate);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            
            var items = JsonSerializer.Deserialize<List<TodoItem>>(File.ReadAllText(Environment.CurrentDirectory + "/feeds.json"), options);
            
            if (File.Exists(Environment.CurrentDirectory + "/Sources.opml") == true)
            {
                var urls = Parser.ParseAtrribute(Environment.CurrentDirectory + "/Sources.opml","xmlUrl");
                foreach (var url in urls)
                {
                    foreach (var post in FeedItems.GetList(url))
                    {
                        if (post.Title != "Error")
                        {
                            var item = new TodoItem { 
                                Title = post.Title,
                                Description = post.Description,
                                Link = post.Link,
                                Data = post.DatePublish
                            };
                            if (items.Contains(item) == false)
                            {
                                items.Add(new TodoItem
                                {
                                    Title = "New" + post.Title,
                                    Description = post.Description,
                                    Link = post.Link,
                                    Data = post.DatePublish
                                });
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Sources.opml file not found in the project folder");
                Environment.Exit(1);
            }

            return items.OrderByDescending(n => n.Data);
        }

        public ObservableCollection<TodoItem> Items { get; }
    }
}