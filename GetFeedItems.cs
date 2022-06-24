using CodeHollow.FeedReader;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using RSS_Avalonia.Models;

namespace Feed
{
    public static class FeedItems
    {
        public static List<FeedPost> GetList(string url)
        {
            var items = new List<FeedPost> {};
            var ping = new Ping();
            var source = new Uri(url);
            
            var isAlive = ping.SendPingAsync(source.Host, 100);
            if (isAlive.Result.Status == IPStatus.Success)
            {
                var readerTask = FeedReader.ReadAsync(url);
                readerTask.ConfigureAwait(false);
                
                foreach (var item in readerTask.Result.Items)
                {
                    items.Add(new FeedPost {Title = item.Title,
                        Description = Regex.Replace(item.Description,"<.*?>|&.*?;", string.Empty).TrimStart('\n'),
                        Link = item.Link,
                        DatePublish = item.PublishingDate
                    });
                }
                
                var orderedNumbers = items.OrderByDescending(n => n.DatePublish);
                
                return orderedNumbers.ToList();
            }
            else
            {
                items.Add(new FeedPost {Title = "Error",
                    Description = $"Во время подключения к {url} произошла ошибка",
                    Link = url,
                    DatePublish = null
                });  
                return items;
            }

        }
    }
}