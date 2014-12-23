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

namespace UWRoomFinder
{
    public partial class FloorsPage : PhoneApplicationPage
    {
        public List<Floor> floors;
        String buildingName;

        public FloorsPage()
        {
            InitializeComponent();

            this.Loaded += FloorsPage_Loaded;

            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarRefreshButtonText;
            ApplicationBar.Buttons.Add(appBarButton);
            appBarButton.Click += RefreshButtonClicked;
        }

        void FloorsPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            floors = new List<Floor>();
            GetFloors();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            buildingName = "";

            if (NavigationContext.QueryString.TryGetValue("b", out buildingName))
            {
                BuildingNameTitle.Text = buildingName;
            }
        }

        private async void GetFloors()
        {
            MainPage.SetProgressIndicator(true);
            SystemTray.ProgressIndicator.Text = "Downloading Floors";

            SortedSet<int> sortedFloors = new SortedSet<int>();

            // Queries database
            var query = from room in ParseObject.GetQuery("StudyRoom")
                        where room.Get<String>("buildingName") == buildingName
                        select room;
            IEnumerable<ParseObject> results = await query.FindAsync();

            foreach (ParseObject o in results)
            {
                string roomN = (string)o["roomNumber"];
                int thisFloorN = -1;
                int.TryParse(roomN.Substring(0, 1), out thisFloorN);
                if (thisFloorN >= 0 && !IsFloorAdded(thisFloorN))
                    sortedFloors.Add(thisFloorN);
            }

            foreach (int f in sortedFloors)
                floors.Add(new Floor(f));

            // Bind to the LongListSelector
            FloorList.ItemsSource = floors;
            MainPage.SetProgressIndicator(false);
        }

        /// <summary>
        /// Checks to see if the given room has been added to the floors list.
        /// </summary>
        /// <param name="roomN"></param>
        /// <returns></returns>
        private bool IsFloorAdded(int thisFloorN)
        {
            if (thisFloorN >= 0)
            {
                foreach (Floor f in floors)
                {
                    if (f.N == thisFloorN)
                        return true;
                }
            }
            return false;
        }

        private void ListSelected(object sender, SelectionChangedEventArgs e)
        {
            if (FloorList.SelectedItem != null)
            {
                Floor f = e.AddedItems[0] as Floor;
                NavigationService.Navigate(new Uri("/RoomsPage.xaml?b=" + buildingName + "&floor=" + f.N, UriKind.Relative));
                FloorList.SelectedItem = null;
            }
        }

        public class Floor
        {
            public int N { get; set; }
            public string FloorN { get; set; }

            public Floor(int n)
            {
                N = n;
                FloorN = "Floor " + n;
            }
        }

        private void RefreshButtonClicked(object sender, EventArgs e)
        {
            GetFloors();
        }
    }
}