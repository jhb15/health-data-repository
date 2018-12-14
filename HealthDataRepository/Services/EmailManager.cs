using HealthDataRepository.Models;
using HealthDataRepository.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthDataRepository.Services
{
    public class EmailManager
    {
        private IEmailRecordRepository emailRecordRepository;
        private IActivityRepository activityRepository;
        private IApiClient apiClient;

        public EmailManager(IEmailRecordRepository emailRecordRepository, IActivityRepository activityRepository, IApiClient apiClient)
        {
            this.emailRecordRepository = emailRecordRepository;
            this.activityRepository = activityRepository;
            this.apiClient = apiClient;
        }

        public async Task SendScheduledEmails()
        {
            await SendActivityUpdateEmails();
            await SendMissedReadingEmails();
        }

        private async Task<List<string>> GetUsersForEmailType(EmailRecordType type)
        {
            List<string> usersToEmail = new List<string>();

            foreach (var userId in await activityRepository.GetUsersWithRecordedActivities())
            {
                var now = DateTime.Now;
                var oneWeekAgo = now.AddDays(-7);
                var lastEmail = await emailRecordRepository.GetLastOfTypeForUserAsync(type, userId);
                if (lastEmail != null && lastEmail.Timestamp > oneWeekAgo)
                {
                    continue;
                }

                var activities = await activityRepository.GetByUserIdAsync(userId, oneWeekAgo, now);
                if (type == EmailRecordType.ACTIVITY_UPDATE && activities.Count > 0)
                {
                    usersToEmail.Add(userId);
                }
                else if (type == EmailRecordType.MISSED_READING_UPDATE && activities.Count == 0)
                {
                    usersToEmail.Add(userId);
                }
            }

            return usersToEmail;
        }

        private async Task SendEmailContent(object content)
        {
            await apiClient.PostAsync("/comms/api/Email/ToUser", content);
        }

        private async Task AddEmailRecord(string userId, EmailRecordType type)
        {
            await emailRecordRepository.AddAsync(new EmailRecord
            {
                UserId = userId,
                Type = type,
                Timestamp = DateTime.Now
            });
        }

        public async Task SendActivityUpdateEmails()
        {
            var users = await GetUsersForEmailType(EmailRecordType.ACTIVITY_UPDATE);
            foreach (var userId in users)
            {
                var now = DateTime.Now;
                var oneWeekAgo = now.AddDays(-7);
                var activities = await activityRepository.GetByUserIdAsync(userId, oneWeekAgo, now);
                var numSteps = activities.Aggregate(0, (acc, x) => acc + x.StepsTaken);
                var caloriesBurnt = activities.Aggregate(0, (acc, x) => acc + x.CaloriesBurnt);
                var totalDistance = activities.Aggregate(0.0, (acc, x) => acc + x.MetresTravelled);

                var emailContent = new StringBuilder();
                emailContent.AppendLine("<p>Hi valued member,</p>");
                emailContent.AppendLine("<p>Looks like you've been busy this week.  Here's a summary of your activities:<p>");
                emailContent.AppendLine("<ul>");
                emailContent.AppendLine($"<li><strong>Steps:</strong> {numSteps}</li>");
                emailContent.AppendLine($"<li><strong>Calories Burnt:</strong> {caloriesBurnt}</li>");
                emailContent.AppendLine($"<li><strong>Total Distance:</strong> {totalDistance} metres</li>");
                emailContent.AppendLine("</ul>");
                emailContent.AppendLine("<p>Have a nice week!</p>");

                await SendEmailContent(new
                {
                    UserId = userId,
                    Subject = "Your weekly activity update",
                    Content = emailContent.ToString()
                });

                await AddEmailRecord(userId, EmailRecordType.ACTIVITY_UPDATE);
            }
        }

        public async Task SendMissedReadingEmails()
        {
            var users = await GetUsersForEmailType(EmailRecordType.MISSED_READING_UPDATE);
            foreach (var userId in users)
            {
                var emailContent = new StringBuilder();
                emailContent.AppendLine("<p>Hi valued member,</p>");
                emailContent.AppendLine("<p>Looks like you haven't recorded any activities this week.<p>");
                emailContent.AppendLine("<p>We hope to see you again soon!</p>");

                await SendEmailContent(new
                {
                    UserId = userId,
                    Subject = "We miss you",
                    Content = emailContent.ToString()
                });

                await AddEmailRecord(userId, EmailRecordType.MISSED_READING_UPDATE);
            }
        }
    }
}
