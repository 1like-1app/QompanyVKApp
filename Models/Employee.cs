using System.Collections.Generic;

namespace QompanyVKApp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        // ReSharper disable once InconsistentNaming
        public string VKId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<EmployeeMeeting> EmployeeMeetings { get; set; }

        public string FullName => $"{FirstName} {LastName}";

    }
}