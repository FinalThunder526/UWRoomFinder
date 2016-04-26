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
using System.Threading.Tasks;

namespace UWRoomFinder
{
    public partial class RoomsPage : PhoneApplicationPage
    {
        String buildingName;
        int floorN;
        public List<StudyRoom> rooms;

        int roomsLoadedCounter = 0;

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

        #region Callbacks
        
        void RoomsPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            rooms = new List<StudyRoom>();
            if (roomsLoadedCounter == 0)
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
            if (roomsLoadedCounter > 0)
            {
                GetRooms();
            }
        }

        #endregion

        #region Button Clicks
        
        private void RefreshButtonClick(object sender, EventArgs e)
        {
            GetRooms();
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
    
        #endregion
        
        /// <summary>
        /// Downloads the rooms asynchronously.
        /// </summary>
        private async void GetRooms()
        {
            roomsLoadedCounter++;
            MainPage.SetProgressIndicator(true);
            SystemTray.ProgressIndicator.Text = "Downloading Rooms";

            // Queries database
            var query = from room in ParseObject.GetQuery("StudyRoom")
                        where room.Get<String>("buildingName") == buildingName
                        where room.Get<String>("roomNumber").StartsWith("" + floorN)
                        select room;
            IEnumerable<ParseObject> results = await query.FindAsync();

            SortedSet<StudyRoom> sortedRooms = new SortedSet<StudyRoom>();
            rooms.Clear();

            for (int i = 0; i < results.Count<ParseObject>(); i++)
            {
                ParseObject o = results.ElementAt<ParseObject>(i);
                string roomN = (string)o["roomNumber"];
                StudyRoom s = new StudyRoom(buildingName, roomN);
                await CheckOutAndSaveStudyRoom(o, s);
                
                sortedRooms.Add(s);
            }

            rooms.Clear();

            foreach (StudyRoom s in sortedRooms) 
                rooms.Add(s);
                        
            RoomList.ItemsSource = rooms;
            MainPage.SetProgressIndicator(false);
        }

        private async Task CheckOutAndSaveStudyRoom(ParseObject o, StudyRoom s)
        {
            DateTime occupiedTill = DateTime.MinValue;
            // See if it's occupied
            try
            {
                occupiedTill = o.Get<DateTime>("occupiedTill");
            }
            catch (Exception e)
            {
                occupiedTill = DateTime.MinValue;
            }
            // If it is, see if the time of departure is past
            if (occupiedTill != DateTime.MinValue)
            {
                // Occupied
                if (occupiedTill.CompareTo(DateTime.UtcNow) <= 0)
                {
                    // Check out!
                    o["occupant"] = null;
                    o["occupiedTill"] = null;
                    await o.SaveAsync();
                    // No longer occupied
                    s.TimeLeft = "Free";
                }
                else
                {
                    // Still occupied
                    s.TimeLeft = o.Get<DateTime>("occupiedTill").Subtract(DateTime.UtcNow).ToString();
                }
            }
            else
            {
                // Not occupied
                s.TimeLeft = "Free";
            }
        }
    }
}