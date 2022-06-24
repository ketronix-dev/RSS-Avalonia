using System.Text.Json;
using Feed;
using OpmlParser;
using ReactiveUI;
using RSS_Avalonia.Models;
using RSS_Avalonia.Services;

namespace RSS_Avalonia.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        ViewModelBase content;
        public MainWindowViewModel(Database db)
        {
            Content = List = new TodoListViewModel(db.Posts());
        }

        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }
        
        public TodoListViewModel List { get; }
        
        public void AddItem()
        {
            foreach (var item in Refresh())
            {
                List.Items.Add(item);
            }
            Content  = List;
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            FileStream fs = new FileStream(Environment.CurrentDirectory + "/feeds.json", FileMode.OpenOrCreate);
            JsonSerializer.SerializeAsync<List<TodoItem>>(fs,List.Items.ToList(),options);
        }
        
        public static IEnumerable<TodoItem> Refresh()
        {
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
                                    Title = post.Title,
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
    }
}