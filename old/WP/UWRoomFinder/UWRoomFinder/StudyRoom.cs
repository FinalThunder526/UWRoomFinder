using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWRoomFinder
{
    public class StudyRoom : IComparable<StudyRoom>
    {
        public string RoomN { get; set; }
        public string BuildingName { get; set; }
        public string Occupant { get; set; }
        public string TimeLeft { get; set; }

        public StudyRoom(string bName, string roomN)
        {
            BuildingName = bName;
            RoomN = roomN;
        }

        public int CompareTo(StudyRoom other)
        {
            return RoomN.CompareTo(other.RoomN);
        }
    }
}
