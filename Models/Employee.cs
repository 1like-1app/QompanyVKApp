using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using QompanyVKApp.Data;
using VkNet.Model;

namespace QompanyVKApp.Models
{
    public class Employee
    {
        public Employee()
            => Meetings = new JoinCollectionFacade<Meeting, EmployeeMeeting>(
                EmployeeMeetings,
                em => em.Meeting,
                t => new EmployeeMeeting {Employee = this, Meeting = t});
        public int Id { get; set; }
        // ReSharper disable once InconsistentNaming
        public string VKId { get; set; }
        public string Photo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<EmployeeMeeting> EmployeeMeetings { get; set; } = new List<EmployeeMeeting>();

        [NotMapped]
        public IEnumerable<Meeting> Meetings { get; }

        public Group Group { get; set; }
        public string FullName => $"{FirstName} {LastName}";

    }
}