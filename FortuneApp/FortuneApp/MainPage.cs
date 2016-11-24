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

            TableView tableView = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot("")
                {
                    new TableSection("FortuneApp")
                    {
                        new ImageCell
                        {

                            ImageSource =
                            Device.OnPlatform(ImageSource.FromUri(new Uri("http://i.imgur.com/JBZYlaU.png")),
                                      ImageSource.FromFile("FortuneApp.png"),
                                      ImageSource.FromUri(new Uri("http://i.imgur.com/JBZYlaU.png"))),
                            Text = "FortuneApp",
                            Detail = "!Your Daily dose of Random Quotes!"
                        }
                    }                   
                }
            };

            label = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Padding = new Thickness(5, Device.OnPlatform(20, 0, 0), 5, 0);

            // Build the page.
            this.Content = new StackLayout
            {                
                Children =
                {
                    tableView,
                    button,
                    label
                    
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

            if (toBeReturned.Length > 800)
            {
                toBeReturned = toBeReturned.Substring(0, 800);
            }

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
            DependencyService.Get<IAudio>().PlayAudioFile("hmmmm.wav");

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