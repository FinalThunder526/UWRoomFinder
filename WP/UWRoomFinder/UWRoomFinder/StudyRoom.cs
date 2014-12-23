using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWRoomFinder
{
    public class StudyRoom
    {
        public string RoomN { get; set; }
        public string BuildingName { get; set; }
        public string Occupant { get; set; }

        public StudyRoom(string bName, string roomN)
        {
            BuildingName = bName;
            RoomN = roomN;
        }
    }
}
