using HealthDataRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthDataRepository.Repositories
{
    public interface IEmailRecordRepository
    {
        Task<EmailRecord> GetLastOfTypeForUserAsync(EmailRecordType type, string userId);

        Task<EmailRecord> AddAsync(EmailRecord record);
    }
}
