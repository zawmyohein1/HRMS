using HRMS.Models.Entities;
using HRMS.Models.Entities.Setup;
using Microsoft.EntityFrameworkCore;

namespace HRMS.DataAccess.Context
{
    public class HRMSDbContext : DbContext
    {
        public HRMSDbContext(DbContextOptions<HRMSDbContext> options)
            : base(options) // Ensures DbContext is initialized with the given options
        {
        }

        // Define DbSets for all entities
        public DbSet<Location> Locations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<JobRole> JobRoles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<JobHistory> JobHistories { get; set; }

        // Configure model relationships using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Employee - Job History (One-to-Many)
            modelBuilder.Entity<JobHistory>()
                .HasOne(jh => jh.Employee)
                .WithMany(e => e.JobHistories)
                .HasForeignKey(jh => jh.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete when Employee is deleted

            // JobRole - Job History (One-to-Many)
            modelBuilder.Entity<JobHistory>()
                .HasOne(jh => jh.JobRole)
                .WithMany()  // Assuming no navigation property on JobRole side
                .HasForeignKey(jh => jh.JobRoleId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete on JobRole, as deleting a role may affect many job histories

            // Manager - Job History (Self-referencing FK, One-to-Many)
            modelBuilder.Entity<JobHistory>()
                .HasOne(jh => jh.Manager)
                .WithMany()
                .HasForeignKey(jh => jh.ManagerId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete on Manager, as a manager could be linked to multiple job histories            

            //SeedData(modelBuilder);

        }


        protected void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Locations
            modelBuilder.Entity<Location>().HasData(
                new Location { Name = "Headquarters", City = "New York", Country = "USA" },
                new Location { Name = "Regional Office", City = "Los Angeles", Country = "USA" },
                new Location { Name = "Branch Office", City = "London", Country = "UK" },
                new Location { Name = "Development Center", City = "Berlin", Country = "Germany" }
            );

            // Seed Departments (Linked to Locations)
            modelBuilder.Entity<Department>().HasData(
                new Department { Name = "HR", Description = "Handles recruitment and payroll.", LocationId = 1 },
                new Department { Name = "IT", Description = "Manages IT infrastructure and software development.", LocationId = 2 },
                new Department { Name = "Sales", Description = "Handles customer sales and business growth.", LocationId = 3 },
                new Department { Name = "R&D", Description = "Research and development of new products.", LocationId = 4 }
            );

            // Seed Job Roles (Linked to Departments)
            modelBuilder.Entity<JobRole>().HasData(
                new JobRole { Title = "HR Manager", Description = "Manages HR operations.", DepartmentId = 1 },
                new JobRole { Title = "Software Engineer", Description = "Develops software solutions.", DepartmentId = 2 },
                new JobRole { Title = "Sales Executive", Description = "Handles sales and customer relations.", DepartmentId = 3 },
                new JobRole { Title = "Research Scientist", Description = "Conducts research and innovation.", DepartmentId = 4 }
            );

            // Seed Employees
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Name = "Alice Johnson",
                    BirthDate = new DateTime(1985, 5, 15),
                    Phone = "123-456-7890",
                    Email = "alice.johnson@example.com",
                    Gender = "Female",
                    Address = "123 Main St, NY",
                    Status = "Active"
                },
                new Employee
                {
                    Name = "Bob Smith",
                    BirthDate = new DateTime(1990, 8, 22),
                    Phone = "987-654-3210",
                    Email = "bob.smith@example.com",
                    Gender = "Male",
                    Address = "456 Elm St, LA",
                    Status = "Active"
                }
            );

            // Seed Job Histories (Linked to Employees and Job Roles)
            modelBuilder.Entity<JobHistory>().HasData(
                new JobHistory
                {
                    Id = 1, // Explicit Id is required by EF Core
                    EmployeeId = 1, // Alice Johnson
                    JobRoleId = 1, // HR Manager
                    StartDate = new DateTime(2020, 1, 10),
                    EndDate = DateTime.MinValue, // Use DateTime.MinValue instead of null
                    ManagerId = 0 // Use 0 or a default value if no manager
                },
                new JobHistory
                {
                    Id = 2, // Explicit Id is required by EF Core
                    EmployeeId = 2, // Bob Smith
                    JobRoleId = 2, // Software Engineer
                    StartDate = new DateTime(2019, 7, 1),
                    EndDate = new DateTime(2023, 2, 15),
                    ManagerId = 1 // Alice is Bob's manager
                }
            );


            base.OnModelCreating(modelBuilder);
        }


    }
}
