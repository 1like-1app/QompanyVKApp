namespace QompanyVKApp.Models
{
    public class EmployeeMeeting
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }
        
    }
}