using System.Collections.Generic;

namespace QompanyVKApp.Models
{
    public class Group
    {
        public int Id { get; set; }
        // ReSharper disable once InconsistentNaming
        public string VKId { get; set; }
        public string AccessToken { get; set; }

        public List<Employee> Employees { get; set; }
        public List<Meeting> Meetings {get; set;}
        public List<MeetingRoom> MeetingRooms {get; set;}

    }
}