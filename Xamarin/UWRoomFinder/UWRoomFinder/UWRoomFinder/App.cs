using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace UWRoomFinder
{
	public class App : Application
	{
        Button b;
        Label l;
        int Count = 0;

        public App()
        {
            b = new Button
            {
                AnchorX = 100,
                AnchorY = 100,
                Text = "Click!"
            }; l = new Label
            {
                XAlign = TextAlignment.Center,
                Text = "Count: " + Count
            };
            // The root page of your application
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
						l, b
					}
                }
            };

            b.Clicked += b_Clicked;
        }

        void b_Clicked(object sender, EventArgs e)
        {
            Count++;
            l.Text = "Count: " + Count;
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
