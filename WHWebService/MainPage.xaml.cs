using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using RestSharp;
using Foundation;
using Xamarin.Forms.Internals;
using System.Net.Http;
using Xamarin.Essentials;

namespace WHWebService
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();


            btnRestSharp.Clicked += (sender, e) => SendUsingRestSharp();
            btnHttpClient.Clicked += async (object sender, EventArgs e) => await SendUsingHttpClient();

        }

        public void SendUsingRestSharp()
        {
            string requestUri = "https://SERVER.azurewebsites.net/api/SpeechToText/SpeechToText?inputLanguage=en-US";

            var client = new RestClient(requestUri);
            var request = new RestRequest(Method.POST);

            var b = File.ReadAllBytes("WAVS/Recording.wav");
            request.AddParameter("audio/wav", b, ParameterType.RequestBody);

            client.ExecuteAsync(request, response => {
                Console.WriteLine("Response: " + response.StatusCode);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    editorResponse.Text = "RestSharp:" + response.Content.ToString();
                });
            });
        }

        public async Task SendUsingHttpClient()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://SERVER.azurewebsites.net");

                var b = File.ReadAllBytes("WAVS/Recording.wav");


                var content = new ByteArrayContent(b); 
                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", "audio/wav");
                HttpResponseMessage response = await client.PostAsync("/api/SpeechToText/SpeechToText?inputLanguage=en-US", content);

                var result = await response.Content.ReadAsStringAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    editorResponse.Text = "HttpClient:" + result;
                });
            }
        }

    }
}
