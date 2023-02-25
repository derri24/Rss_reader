using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Webkit;
using Android.Widget;
using Rss_reader.rss_client.Models;

namespace Rss_reader
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var searchBtn = (Button) FindViewById(Resource.Id.searchBtn);
            if (searchBtn != null)
                searchBtn.Click += delegate { SearchBtn_Click(); };
        }

        private async Task SearchBtn_Click()
        {
            var source = ((EditText) FindViewById(Resource.Id.@ref))?.Text;
            HttpClient client = new HttpClient();
            var response = client.GetAsync("https://www.onliner.by/feed").Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                var xml = await responseContent.ReadAsStringAsync();

                XmlSerializer serializer = new XmlSerializer(typeof(RssModel));
                TextReader reader = new StringReader(xml);
                var rssModel = (RssModel) serializer.Deserialize(reader);


                WebView webView = (WebView) FindViewById(Resource.Id.webview);
// static html string data
                string customHtml = "<html><body><h1>Hello, AbhiAndroid</h1>" +
                                    "<h1>Heading 1</h1><h2>Heading 2</h2><h3>Heading 3</h3>" +
                                    "<p>This is a sample paragraph of static HTML In Web view</p>" +
                                    "</body></html>";
// load static html data on a web view
                if (webView != null)
                    webView.LoadData(customHtml, "text/html", "UTF-8");
            }
        }
    }
}