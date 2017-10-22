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
                // Look for any employees.
                if (context.Employees.Any())
                {
                    return;   // DB has been seeded
                }
                var group = new Group
                {
                    AccessToken =
                        "40099039dc84bbba636fff7051de15181a2da99a439ae66557e5646a64f36d75faebb023eb4cc7e2d86f6",
                    VKId = "155492790"
                };

                var room = new MeetingRoom() { Name = "Place where people talk about tfs", Floor = 2, Capacity = 5,  Group = group};
                
                var meetings = new List<Meeting>()
                    {
                        new Meeting
                        {
                            Theme = "Talk about why Git is better than TFVS",
                            StartTime = DateTime.Now,
                            EndTime = DateTime.Now + TimeSpan.FromHours(1),
                            MeetingRoom = room,
                            Group = group
                        }
                    };

                var employees = new List<Employee>()
                {
                    new Employee
                    {
                        FirstName = "Sergey",
                        LastName = "Polezhaev",
                        Group = group
                    },
                    new Employee
                    {
                        FirstName = "Yury",
                        LastName = "Belousov",
                        Group = group
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
                room.Meetings = new List<Meeting> { meetings[0] };
                context.Meetings.AddRange(meetings);
                context.SaveChanges();
            }
        }
    }
}