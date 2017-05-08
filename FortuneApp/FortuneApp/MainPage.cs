using System;
using System.IO;
using System.Net;
using Xamarin.Forms;

namespace FortuneApp
{
    public class MainPage : ContentPage
    {
        Label label = new Label
        {
            Text = "Get Ready For Some Fortune!",
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            VerticalOptions = LayoutOptions.CenterAndExpand
        };


        public MainPage()
        {

            Button button = new Button
            {
                Text = "Get Some Fortune!",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            button.Clicked += OnButtonClicked;

            Image logo = new Image
            {
                Source =
                            Device.OnPlatform(ImageSource.FromUri(new Uri("http://i.imgur.com/iMFnuiy.png")),
                                      ImageSource.FromFile("Logo.png"),
                                      ImageSource.FromUri(new Uri("http://i.imgur.com/iMFnuiy.png"))),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            Label header = new Label
            {
                Text = "Fortune App",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            Label motto = new Label
            {
                Text = "Your daily dose of random quotes",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            label = new Label
            {
                Text = "Memento Mori",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            ScrollView textHandler = new ScrollView
            {
                Content = label,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            BoxView justSpace = new BoxView
            {
                Color = Color.Transparent,
                HeightRequest = 20
            };

            // Build the page.
            this.Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 20,
                Children =
                {
                    header,
                    logo,
                    motto,
                    justSpace,
                    textHandler,
                    button                    
                }
            };


        }

        static string MakeItProper(string toBeEnchanted)
        {
            string toBeReturned = toBeEnchanted.Replace("\\t", "\n");

            toBeReturned = toBeReturned.Replace("\\n", "\n");
            toBeReturned = toBeReturned.Replace("\\", "");
            toBeReturned = toBeReturned.Replace("Q:\n", "Q:\t");
            toBeReturned = toBeReturned.Replace("A:\n", "A:\t");
            toBeReturned = toBeReturned.Replace("\n\n", "\n");

            toBeReturned = toBeReturned.Substring(1, toBeReturned.Length - 2);

            if (toBeReturned.Equals("Rate Limit Exceeded"))
            {
                toBeReturned = "Memento Mori!";
            }

            if (toBeReturned.Length == 0)
            {
                toBeReturned = "We need internet for our relationship to Work!";
            }

            return toBeReturned;
        }

        void OnButtonClicked(object sender, EventArgs args)
        {
            DependencyService.Get<IAudio>().PlayAudioFile("hmm.wav");

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

                        toBeShown = MakeItProper(toBeShown);

                        label.Text = toBeShown;
                    });
                }
                catch (Exception exc)
                {
                    exc.ToString();
                }
            }, null);
        }
    }
}