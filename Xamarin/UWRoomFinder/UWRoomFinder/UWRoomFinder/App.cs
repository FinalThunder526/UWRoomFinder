using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

using Parse;

namespace UWRoomFinder
{
    public class App : Application
    {
        public App()
        {
            this.MainPage = new UWRoomFinder.MainPage();
            ParseClient.Initialize("L3Toj2w8dfPshDHJX1sqDWVcT4enSvOusLnxJo5f", "Np86OZyFW3Ga4yH73kfjAvG92XiDrueV4nG1IE99");
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }
        
        protected override void OnSleep()
        {
            base.OnSleep();
        }
    }
}
