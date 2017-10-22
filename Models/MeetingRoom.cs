using System.Collections.Generic;

namespace QompanyVKApp.Models
{
    public class MeetingRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Floor { get; set; }
        public Group Group { get; set; }
        public int? Capacity { get; set; }
        public List<Meeting> Meetings { get; set; }
    }
}