using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Infastructure;
using TaskManager.Infastructure.Services;


namespace TaskManager.Application.Services
{
    public class TaskNotificationService:BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TaskNotificationService> _logger;
        public TaskNotificationService(IServiceProvider serviceProvider, ILogger<TaskNotificationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
         
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TaskNotificationService started.");

            while (!stoppingToken.IsCancellationRequested)
            { 

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<TaskManagerDBContext>();
                        //var smsService = scope.ServiceProvider.GetRequiredService<SmsService>();
                        var emailService = scope.ServiceProvider.GetRequiredService<TaskManagerEmailService>();

                        // Find tasks due tomorrow
                        var tomorrow = DateTime.UtcNow.AddDays(1).Date;
                        //var tasksDueTomorrow = dbContext.TaskItems
                        //    .Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == tomorrow)
                        //    .ToList();
                        var tasksDueTomorrow = await dbContext.Set<TaskDueTommorrowDto>().FromSqlRaw("EXEC GET_TASK_DUE_TOMMOROW").ToListAsync();

                        foreach (var task in tasksDueTomorrow)
                        {
                            // Send WhatsApp message
                            //var isSenst = await smsService.SendSms(task.PhoneNumber, $"Reminder: Task '{task.Title}' is due tomorrow!");
                            var isSend = await emailService.SendEmailAsync(task.Email, "Task reminder!", $"<div>Reminder: Task: '{task.Title}' is due tomorrow!</div><div style='margin-top:32px; color:red'>Ignore, this is a test email from Adwait Kulkarni</div>");
                            _logger.LogInformation($"Sent WhatsApp reminder for task {task.Name} to {task.Name}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending task notifications.");
                }

                // Wait 24 hours before next check (adjust as needed)
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
    }
}
