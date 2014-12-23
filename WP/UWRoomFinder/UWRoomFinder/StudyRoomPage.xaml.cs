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

        private void LoadViews()
        {
            string currentOccupant = (string)RoomObject["occupant"];
            if (currentOccupant == null || currentOccupant.Trim().Length <= 0)
            {
                // FREE
                FreeView.Visibility = System.Windows.Visibility.Visible;
                FreeView.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                // OCCUPIED
            }
                
        }

        private void CheckInButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            newOccupant = NewOccupantBlock.Text;
            if (newOccupant != null && newOccupant.Trim().Length > 0)
            {
                CheckIn();
            }
        }

        private async void CheckIn()
        {
            RoomObject["occupant"] = newOccupant;

            DateTime now = DateTime.UtcNow;
            DateTime free = now.AddMinutes(newTime);
            RoomObject["occupiedTill"] = free;

            await RoomObject.SaveAsync();
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