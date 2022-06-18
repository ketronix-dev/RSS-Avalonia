using CodeHollow.FeedReader;
using Pastel;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace Feed
{
    public static class FeedItems
    {
        public static List<string[]> GetList(string url)
        {
            var items = new List<string[]> {};
            var ping = new Ping();

            var source = new Uri(url);
            
            var isAlive = ping.SendPingAsync(source.Host, 500);
            if (isAlive.Result.Status == IPStatus.Success)
            {
                var readerTask = FeedReader.ReadAsync(url);
                readerTask.ConfigureAwait(false);
                
                foreach (var item in readerTask.Result.Items)
                {
                    Console.WriteLine(item.PublishingDate);
                    string[] post = {item.Title, Regex.Replace(item.Description,"<.*?>|&.*?;", string.Empty).TrimStart('\n'), item.Link};
                    items.Add(post); //Pastel("#116bba")  Pastel("#11ba79")
                }
                return items;
            }
            else
            {
                string[] error = { "Ошибка. Нет связи с источником фидов." };
                items.Add(error);  
                return items;
            }

        }
    }
}