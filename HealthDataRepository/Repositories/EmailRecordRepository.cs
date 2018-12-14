using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthDataRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthDataRepository.Repositories
{
    public class EmailRecordRepository : IEmailRecordRepository
    {
        private HealthDataRepositoryContext context;

        public EmailRecordRepository(HealthDataRepositoryContext context)
        {
            this.context = context;
        }

        public async Task<EmailRecord> AddAsync(EmailRecord record)
        {
            context.EmailRecord.Add(record);
            await context.SaveChangesAsync();
            return record;
        }

        public async Task<EmailRecord> GetLastOfTypeForUserAsync(EmailRecordType type, string userId)
        {
            return await context.EmailRecord
                .Where(er => er.Type == type)
                .Where(er => er.UserId == userId)
                .OrderByDescending(er => er.Timestamp)
                .FirstOrDefaultAsync();
        }
    }
}
