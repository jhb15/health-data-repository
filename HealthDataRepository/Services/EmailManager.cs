using HealthDataRepository.Models;
using HealthDataRepository.Repositories;
using Microsoft.Extensions.Logging;
using System;
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

        public async Task SendActivityUpdateEmails()
        {
            var users = await activityRepository.GetUsersWithRecordedActivities();
            foreach (var userId in users)
            {
                var now = DateTime.Now;
                var oneWeekAgo = now.AddDays(-7);
                var lastEmail = await emailRecordRepository.GetLastOfTypeForUserAsync(EmailRecordType.ACTIVITY_UPDATE, userId);
                if (lastEmail != null && lastEmail.Timestamp > oneWeekAgo)
                {
                    continue;
                }

                var activities = await activityRepository.GetByUserIdAsync(userId, oneWeekAgo, now);
                if (activities.Count == 0)
                {
                    continue;
                }

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

                await apiClient.PostAsync("/comms/api/Email/ToUser", new
                {
                    UserId = userId,
                    Subject = "Your weekly activity update",
                    Content = emailContent.ToString()
                });

                await emailRecordRepository.AddAsync(new EmailRecord
                {
                    UserId = userId,
                    Type = EmailRecordType.ACTIVITY_UPDATE,
                    Timestamp = now
                });
            }
        }

        public Task SendMissedReadingEmails()
        {
            return Task.CompletedTask;
        }
    }
}
