namespace QompanyVKApp.Models
{
    /// <summary>
    /// Many-to-many relationship between Employee and Meeting
    /// Because 1 Employee can participate in many Meetings &
    /// In 1 Meeting can participate many Employees
    /// </summary>
    /// <remarks>
    /// We really want if it could be done autimatic, but EF Core doesn't support it
    /// </remarks>
    /// <seealso cref="https://github.com/aspnet/EntityFrameworkCore/issues/1368"/>
    public class EmployeeMeeting
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }
        
    }
}