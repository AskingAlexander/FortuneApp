using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

namespace FortuneApp
{
    public class MainPage : ContentPage
    {
        Label label;

        public MainPage()
        {
            Button button = new Button
            {
                Text = "Get Some Fortune!",
                HorizontalOptions = LayoutOptions.Center
            };
            button.Clicked += OnButtonClicked;

            label = new Label();
            Padding = new Thickness(5, Device.OnPlatform(20, 0, 0), 5, 0);
            Content = new StackLayout
            {
                Children =
                    {
                        button,
                        new ScrollView
                        {
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            Content = label,
                            HorizontalOptions = LayoutOptions.Center
                        }
                    }
            };
        }
        void OnButtonClicked(object sender, EventArgs args)
        {

            Uri uri = new Uri("https://helloacm.com/api/fortune/");
            WebRequest request = WebRequest.Create(uri);
            request.BeginGetResponse((result) =>
            {
                try
                {
                    Stream stream = request.EndGetResponse(result).GetResponseStream();
                    StreamReader reader = new StreamReader(stream);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        string toBeShown = reader.ReadToEnd();

                        toBeShown = toBeShown.Replace("\\t", "\n");
                        toBeShown = toBeShown.Replace("\\n", "\n");
                        toBeShown = toBeShown.Replace("\\", "");

                        toBeShown = toBeShown.Substring(1, toBeShown.Length - 2);
                        if (toBeShown.Equals("Rate Limit Exceeded"))
                        {
                            toBeShown = "Memento Mori!";
                        }
                            label.Text = toBeShown;
                    });
                }
                catch (Exception exc)
                {
                }
            }, null);
        }
    }
}