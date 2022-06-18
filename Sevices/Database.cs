using Avalonia;
using RSS_Avalonia.Models;
using Feed;
using MessageBox.Avalonia;
using OpmlParser;
using Microsoft.CodeAnalysis.CSharp;

namespace RSS_Avalonia.Services
{
    public class Database
    {
        public IEnumerable<TodoItem> GetItems() => Posts();

        public IEnumerable<TodoItem> Posts()
        {
            var items = new List<TodoItem> { };
            if (File.Exists(Environment.CurrentDirectory + "/Sources.opml") == true)
            {
                var urls = Parser.ParseAtrribute(Environment.CurrentDirectory + "/Sources.opml","xmlUrl");
                foreach (var url in urls)
                {
                    foreach (var post in FeedItems.GetList(url))
                    {
                        if (post.Length == 3)
                        {
                            items.Add(new TodoItem { Title = post[0], Description = post[1], Link = post[2]});   
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Sources.opml file not found in the project folder");
                Environment.Exit(1);
            }

            return items.ToArray();
        }
    }
}