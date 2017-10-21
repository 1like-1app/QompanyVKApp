using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QompanyVKApp.Models;

namespace QompanyVKApp.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new QompanyDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<QompanyDbContext>>()))
            {
                // Look for any movies.
                if (context.Employees.Any())
                {
                    return;   // DB has been seeded
                }
                var room = new MeetingRoom() { Name = "tfs", Floor = 2, Capacity = 5 };
                //context.MeetingRooms.Add(room);
                var meetings = new List<Meeting>()
                    {
                        new Meeting
                        {
                            Theme = "Talk about why Git is better than TFVS",
                            StartTime = DateTime.Now,
                            EndTime = DateTime.Now + TimeSpan.FromHours(1),
                            MeetingRoom = room
                        }
                    };

                var employees = new List<Employee>()
                {
                    new Employee
                    {
                        FirstName = "Segey",
                        LastName = "Polezhaev"
                    },
                    new Employee
                    {
                        FirstName = "Yury",
                        LastName = "Belousov"
                    }
                };
                meetings[0].EmployeeMeetings = new List<EmployeeMeeting>()
                {
                    new EmployeeMeeting
                    {
                        Meeting = meetings[0],
                        Employee = employees[0]
                    },
                    new EmployeeMeeting
                    {
                        Meeting = meetings[0],
                        Employee = employees[1]
                    }
                };

                context.Meetings.AddRange(meetings);
                context.SaveChanges();
            }
        }
    }
}