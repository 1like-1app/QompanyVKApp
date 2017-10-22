using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using QompanyVKApp.Data;
using VkNet.Model;

namespace QompanyVKApp.Models
{
    public class Meeting
    {
        public Meeting()
            => Employees = new JoinCollectionFacade<Employee, EmployeeMeeting>(
                EmployeeMeetings,
                em => em.Employee,
                p => new EmployeeMeeting {Employee = p, Meeting = this});
        public int Id { get; set; }
        public string Theme { get; set; }
        public MeetingRoom MeetingRoom { get; set; }
        public Group Group {get; set;}
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<EmployeeMeeting> EmployeeMeetings { get; set; } = new List<EmployeeMeeting>();

        [NotMapped]
        public IEnumerable<Employee> Employees { get; }

    }
}