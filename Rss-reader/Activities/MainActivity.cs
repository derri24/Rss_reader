using System.Collections.Generic;
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


namespace Rss_reader
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var searchBtn = (Button) FindViewById(Resource.Id.urlBtn);
            if (searchBtn != null)
                searchBtn.Click += delegate { SearchBtn_Click(); };
        }

        private async Task SearchBtn_Click()
        {
            var source = ((EditText) FindViewById(Resource.Id.url))?.Text;
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
         
                var content=HtmlCreator.GetContent(rssModel);
                if (webView != null)
                    webView.LoadData(content, "text/html", "UTF-8");
            }
        }
    }
}