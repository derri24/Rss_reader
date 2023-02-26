using System;

namespace Rss_reader
{
    public static class HtmlCreator
    {
        public static string GetContent(RssModel rssModel)
        {
            string content = "";
            foreach (var item in rssModel.Channel.Items)
            {
                content +=$"<h2>{item.Creator}</h2>" +
                          $"<h3>{item.Title}</h3>" +
                          $"<p><i>{item.PubDate.Split("+")[0]}</i></p>" +
                         
                          $"<p>{item.Description}</p>"+
                          "</br>";
            }
            return
                $"<html><body> {content} </body></html>";
        }
    }
}