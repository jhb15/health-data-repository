using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HealthDataRepository.Services
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> GetAsync(string path);

        Task<HttpResponseMessage> PostAsync(string path, object body);
    }
}
