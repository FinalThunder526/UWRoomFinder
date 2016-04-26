using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace UWRoomFinder
{
	public class MainPage : ContentPage
	{
        Button b;
        Label l;
        ListView list;
        int Count = 0;
        IList<string> buildingsList;

        public MainPage()
        {
            buildingsList = new List<string>();
            buildingsList.Add("Alder");
            b = new Button
            {
                AnchorX = 100,
                AnchorY = 100,
                Text = "Click!"
            };
            l = new Label
            {
                XAlign = TextAlignment.Center,
                Text = "Count: " + Count
            };
            list = new ListView
            {
                ItemsSource = buildingsList
            };
            
            // The root page of your application
            Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
						l, b, list
					}
                };

            b.Clicked += b_Clicked;
            list.ItemTapped += list_ItemTapped;
        }

        void list_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new SecondPage());
        }

        void b_Clicked(object sender, EventArgs e)
        {
            Count++;
            l.Text = "Count: " + Count;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
	}
}
