﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using QompanyVKApp.Data;
using System;

namespace QompanyVKApp.Migrations
{
    [DbContext(typeof(QompanyDbContext))]
    [Migration("20171020194614_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("QompanyVKApp.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("VKId");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("QompanyVKApp.Models.EmployeeMeeting", b =>
                {
                    b.Property<int>("EmployeeId");

                    b.Property<int>("MeetingId");

                    b.HasKey("EmployeeId", "MeetingId");

                    b.HasIndex("MeetingId");

                    b.ToTable("EmployeeMeetings");
                });

            modelBuilder.Entity("QompanyVKApp.Models.Meeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndTime");

                    b.Property<int?>("MeetingRoomId");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("Id");

                    b.HasIndex("MeetingRoomId");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("QompanyVKApp.Models.MeetingRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Capacity");

                    b.Property<int?>("Floor");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("MeetingRooms");
                });

            modelBuilder.Entity("QompanyVKApp.Models.EmployeeMeeting", b =>
                {
                    b.HasOne("QompanyVKApp.Models.Employee", "Employee")
                        .WithMany("EmployeeMeetings")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("QompanyVKApp.Models.Meeting", "Meeting")
                        .WithMany("EmployeeMeetings")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QompanyVKApp.Models.Meeting", b =>
                {
                    b.HasOne("QompanyVKApp.Models.MeetingRoom", "MeetingRoom")
                        .WithMany()
                        .HasForeignKey("MeetingRoomId");
                });
#pragma warning restore 612, 618
        }
    }
}
