﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository.Context;

namespace Repository.Migrations
{
    [DbContext(typeof(ActivityDbContext))]
    [Migration("20201203031201_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Domain.Entitys.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DetailsFinish")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("ExpectecStartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("FinishDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("FinishMaximum")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("TypeActivity")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Activitys");
                });

            modelBuilder.Entity("Domain.Entitys.ActivityTechnology", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ActivityId")
                        .HasColumnType("int");

                    b.Property<int>("TechnologyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("TechnologyId");

                    b.ToTable("ActivityTechnologies");
                });

            modelBuilder.Entity("Domain.Entitys.Technology", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Technologies");
                });

            modelBuilder.Entity("Domain.Entitys.TimeActivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ActivityId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateInitial")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Finished")
                        .HasColumnType("tinyint(1)");

                    b.Property<TimeSpan>("TimeSpan")
                        .HasColumnType("time(6)");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.ToTable("TimeActivities");
                });

            modelBuilder.Entity("Domain.Entitys.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("NextPasswordUpdate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Password")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128) CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Entitys.Activity", b =>
                {
                    b.HasOne("Domain.Entitys.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entitys.ActivityTechnology", b =>
                {
                    b.HasOne("Domain.Entitys.Activity", "Activity")
                        .WithMany("ActivityTechnologies")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entitys.Technology", "Technology")
                        .WithMany("ActivityTechnologies")
                        .HasForeignKey("TechnologyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");

                    b.Navigation("Technology");
                });

            modelBuilder.Entity("Domain.Entitys.TimeActivity", b =>
                {
                    b.HasOne("Domain.Entitys.Activity", "Activity")
                        .WithMany("TimeActivities")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");
                });

            modelBuilder.Entity("Domain.Entitys.Activity", b =>
                {
                    b.Navigation("ActivityTechnologies");

                    b.Navigation("TimeActivities");
                });

            modelBuilder.Entity("Domain.Entitys.Technology", b =>
                {
                    b.Navigation("ActivityTechnologies");
                });
#pragma warning restore 612, 618
        }
    }
}