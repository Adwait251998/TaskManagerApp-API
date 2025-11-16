using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.ViewModels;
using TaskManager.Core.Entities;

namespace TaskManager.Infastructure
{
    public class TaskManagerDBContext: DbContext
    {
        public TaskManagerDBContext(DbContextOptions<TaskManagerDBContext>options):base(options) 
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<TaskDueTommorrowDto> TasksDueTomorrow { get; set; }

        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
           .HasMany(u => u.Tasks)
           .WithOne(t => t.User)
           .HasForeignKey(t => t.UserId);
            modelBuilder.Entity<TaskDueTommorrowDto>().HasNoKey().ToView(null);

            modelBuilder.Entity<Status>()
           .HasMany(s => s.TaskItems)
           .WithOne(t => t.Status)
           .HasForeignKey(t => t.StatusId);



           modelBuilder.Entity<Status>().HasData(
             new Status { Id = 1, Name = "Pending", Description = "Waiting to start!" },
             new Status { Id = 2, Name = "In Progress", Description = "Task is ongoing!" },
             new Status { Id = 3, Name = "Completed", Description = "Task finished!" },
             new Status { Id = 4, Name = "Cancelled", Description = "Task cancelled!" }
            );
            modelBuilder.Entity<TaskItemViewModel>().HasNoKey();
        }
    }
}
