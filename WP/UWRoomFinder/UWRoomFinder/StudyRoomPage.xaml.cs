using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Globalization;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Parse;

namespace UWRoomFinder
{
    public partial class StudyRoomPage : PhoneApplicationPage
    {
        StudyRoom Room;
        ParseObject RoomObject;

        int newTime = 15;
        String newOccupant;

        public StudyRoomPage()
        {
            InitializeComponent();
            this.Loaded += StudyRoomPage_Loaded;
        }

        void StudyRoomPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            LoadRoom();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            String r, b;

            NavigationContext.QueryString.TryGetValue("r", out r);
            NavigationContext.QueryString.TryGetValue("b", out b);
            if (r != null && b != null)
            {
                Room = new StudyRoom(b, r);
            }

            RoomTitle.Text = b + " " + r;
        }

        private async void LoadRoom()
        {
            MainPage.SetProgressIndicator(true);
            SystemTray.ProgressIndicator.Text = "Downloading Room";

            // Queries database
            var query = from r in ParseObject.GetQuery("StudyRoom")
                        where r.Get<String>("roomNumber") == Room.RoomN
                        select r;
            IEnumerable<ParseObject> results = await query.FindAsync();

            RoomObject = results.First<ParseObject>();
            DescriptionBlock.Text = (string)RoomObject["description"];

            LoadViews();

            MainPage.SetProgressIndicator(false);
        }

        string currentOccupant = null;
            
        /// <summary>
        /// Once the StudyRoom has been fetched, loads the views depending on whether
        /// it is free or not.
        /// </summary>
        private void LoadViews()
        {
            try
            {
                currentOccupant = (string)RoomObject["occupant"];
            }
            catch (KeyNotFoundException e)
            {
                currentOccupant = null;
            }
            if (currentOccupant == null || currentOccupant.Trim().Length <= 0)
            {
                // FREE
                FreeView.Visibility = Visibility.Visible;
                OccupiedView.Visibility = Visibility.Collapsed;
            }
            else
            {
                // OCCUPIED
                FreeView.Visibility = Visibility.Collapsed;
                OccupiedView.Visibility = Visibility.Visible;
                CurrentOccupantBlock.Text = currentOccupant;
                DateTime occupiedTill = RoomObject.Get<DateTime>("occupiedTill");
                OccupiedTillBlock.Text = occupiedTill.ToShortTimeString();
            }                
        }

        private void CheckInButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            newOccupant = NewOccupantBlock.Text;
            if (newOccupant != null && newOccupant.Trim().Length > 0)
            {
                if(currentOccupant == null)
                    CheckIn();
            }
        }

        private async void CheckIn()
        {
            RoomObject["occupant"] = newOccupant;

            DateTime now = DateTime.UtcNow;
            DateTime free = now.AddMinutes(newTime);
            RoomObject["occupiedTill"] = free;

            MainPage.SetProgressIndicator(true);
            SystemTray.ProgressIndicator.Text = "Checking in...";

            try
            {
                await RoomObject.SaveAsync();
            }
            catch (Exception e)
            {
                // Unable to check in
            }
            MainPage.SetProgressIndicator(false);

            LoadViews();
        }

        private void Subtract15_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(newTime > 15)
                newTime -= 15;
            NewTimeText.Text = "" + newTime;
        }

        private void Add15_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(newTime < 120)
                newTime += 15;
            NewTimeText.Text = "" + newTime;
        }
    }
}