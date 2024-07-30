﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Api.Responses;
using System.Text;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Infrastructure.Services.SessionStorage
{
    public class SessionStorageService : ISessionStorageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionStorageService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ProviderResponse ProviderResponse
        {
            get
            {
                return Get<ProviderResponse>(nameof(ProviderResponse));
            }
            set
            {
                if (value == null)
                {
                    Remove(nameof(ProviderResponse));
                }
                else
                {
                    Set(nameof(ProviderResponse), value);
                }
            }
        }

        private T Get<T>(string key) where T : class
        {
            var value = _httpContextAccessor.HttpContext.Session.GetString(key);

            return string.IsNullOrWhiteSpace(value) ? default : JsonConvert.DeserializeObject<T>(value);
        }

        private void Set<T>(string key, T value) where T : class
        {
            var serializedValue = JsonConvert.SerializeObject(value);
            _httpContextAccessor.HttpContext.Session.SetString(key, serializedValue);
        }

        private string Get(string key)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(key);
        }

        private void Set(string key, string stringValue)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, stringValue);
        }

        private void Remove(string key)
        {
            _httpContextAccessor.HttpContext.Session.Remove(key);
        }
    }
}
