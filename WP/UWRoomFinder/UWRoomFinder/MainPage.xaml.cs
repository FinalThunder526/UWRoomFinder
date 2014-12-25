using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using UWRoomFinder.Resources;
using Parse;
using System.Collections;

namespace UWRoomFinder
{
    public partial class MainPage : PhoneApplicationPage
    {
        public List<Building> buildings;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            this.Loaded += MainPage_Loaded;

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            buildings = new List<Building>();
            GetBuildings();
        }

        private async void GetBuildings()
        {
            SetProgressIndicator(true);
            SystemTray.ProgressIndicator.Text = "Downloading Buildings";

            SortedSet<string> sortedBuildings = new SortedSet<string>();

            try
            {
                // Queries the Parse database
                var query = ParseObject.GetQuery("StudyRoom");
                IEnumerable<ParseObject> results = await query.FindAsync();

                SystemTray.ProgressIndicator.Text = "Downloaded";

                buildings.Clear();
                foreach (ParseObject o in results)
                {
                    string b = (string)o["buildingName"];
                    if (!IsBuildingAdded(b))
                        sortedBuildings.Add(b);
                }

                foreach (string b in sortedBuildings)
                {
                    buildings.Add(new Building(b));
                }
            }
            catch (ParseException e)
            {
                SystemTray.ProgressIndicator.Text = "Download failed.";
            }
            SetProgressIndicator(false);
            BuildingLongList.ItemsSource = buildings;
            BuildingLongList.Visibility = Visibility.Visible;
        }

        private bool IsBuildingAdded(string name)
        {
            foreach (Building b in buildings)
            {
                if (b.BuildingName.Equals(name))
                    return true;
            }
            return false;
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarRefreshButtonText;
            ApplicationBar.Buttons.Add(appBarButton);
            appBarButton.Click += RefreshButtonClicked;

            // Create a new menu item with the localized string from AppResources.
            //ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            //ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        public static void SetProgressIndicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }

        private void RefreshButtonClicked(object sender, EventArgs e)
        {
            GetBuildings();            
        }

        public class Building
        {
            public string BuildingName { get; set; }
            public int NOfFloors { get; set; }

            public Building(string bname)
            {
                BuildingName = bname;
            }
        }

        private void ListSelected(object sender, SelectionChangedEventArgs e)
        {
            if (BuildingLongList.SelectedItem != null)
            {
                Building b = e.AddedItems[0] as Building;
                NavigationService.Navigate(new Uri("/FloorsPage.xaml?b=" + b.BuildingName, UriKind.Relative));
                BuildingLongList.SelectedItem = null;
            }
        }
    }
}