using System;
using System.Collections.Generic;

namespace QompanyVKApp.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public MeetingRoom MeetingRoom { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<EmployeeMeeting> EmployeeMeetings { get; set; }
    }
}