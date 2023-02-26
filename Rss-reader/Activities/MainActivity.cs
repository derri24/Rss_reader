using System;
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
using Java.IO;
using Rss_reader.Serializers;
using Xamarin.Essentials;


namespace Rss_reader
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            var urlBtn = (Button) FindViewById(Resource.Id.urlBtn);
            if (urlBtn != null)
                urlBtn.Click += delegate { SearchBtn_Click(); };

            var fileBtn = (Button) FindViewById(Resource.Id.fileBtn);
            if (fileBtn != null)
                fileBtn.Click += delegate { FileBtn_Click(); };
        }

        private void LoadDataToWebView(string xml)
        {
            var rssModel = XMLSerializer.Deserialize(xml);
            var content = HtmlCreator.GetContent(rssModel);
            WebView webView = (WebView) FindViewById(Resource.Id.webview);
            if (webView != null)
                webView.LoadData(content, "text/html", "UTF-8");
        }

        private async Task FileBtn_Click()
        {
            try
            {
                var result = await FilePicker.PickAsync();

                StreamReader streamReader = new StreamReader(result.FullPath);
                var xml = streamReader.ReadToEndAsync().Result;
                streamReader.Close();
                LoadDataToWebView(xml);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        private async Task SearchBtn_Click()
        {
            var source = ((EditText) FindViewById(Resource.Id.url))?.Text;
            HttpClient client = new HttpClient();
            var response = client.GetAsync(source).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                var xml = await responseContent.ReadAsStringAsync();
                LoadDataToWebView(xml);
            }
        }
    }
}