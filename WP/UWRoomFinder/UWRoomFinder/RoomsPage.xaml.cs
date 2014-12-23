using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Parse;
using UWRoomFinder.Resources;

namespace UWRoomFinder
{
    public partial class RoomsPage : PhoneApplicationPage
    {
        String buildingName;
        int floorN;
        public List<StudyRoom> rooms;

        public RoomsPage()
        {
            InitializeComponent();
            this.Loaded += RoomsPage_Loaded;
            BuildLocalizedApplicationBar();
        }
        
        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarRefreshButtonText;
            ApplicationBar.Buttons.Add(appBarButton);
            appBarButton.Click += RefreshButtonClick;
        }

        private void RefreshButtonClick(object sender, EventArgs e)
        {
            GetRooms();
        }

        void RoomsPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            rooms = new List<StudyRoom>();
            GetRooms();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationContext.QueryString.TryGetValue("b", out buildingName);
            String floor;
            NavigationContext.QueryString.TryGetValue("floor", out floor);

            if (buildingName != null && floor != null)
            {
                int.TryParse(floor, out floorN);
                PageTitle.Text = buildingName + " " + floorN;
            }
        }

        private async void GetRooms()
        {
            MainPage.SetProgressIndicator(true);
            SystemTray.ProgressIndicator.Text = "Downloading Rooms";

            SortedSet<string> sortedRooms = new SortedSet<string>();

            // Queries database
            var query = from room in ParseObject.GetQuery("StudyRoom")
                        where room.Get<String>("buildingName") == buildingName
                        where room.Get<String>("roomNumber").StartsWith("" + floorN)
                        select room;
            IEnumerable<ParseObject> results = await query.FindAsync();

            foreach (ParseObject o in results )
            {
                string roomN = (string)o["roomNumber"];
                sortedRooms.Add(roomN);
            }

            foreach (string n in sortedRooms)
                rooms.Add(new StudyRoom(buildingName, n));

            RoomList.ItemsSource = rooms;
            MainPage.SetProgressIndicator(false);
        }

        private void ListSelected(object sender, SelectionChangedEventArgs e)
        {
            if (RoomList.SelectedItem != null)
            {
                StudyRoom r = e.AddedItems[0] as StudyRoom;
                NavigationService.Navigate(new Uri("/StudyRoomPage.xaml?b=" + buildingName + "&r=" + r.RoomN, UriKind.Relative));
                RoomList.SelectedItem = null;
            }
        }
    }
}