using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBasedAuth_NetCore.Extensions;

namespace TokenBasedAuth_NetCore.Utilities
{
    public class SessionService : ISessionService
    {
        private static IHttpContextAccessor _httpContextAccessor;
        private static ISession _session => _httpContextAccessor.HttpContext.Session;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }
        public T GetObject<T>(string sessionKey)
        {
            return _session.GetObject<T>(sessionKey);
        }

        public bool IsNull(string sessionKey)
        {
            return _session.GetString(sessionKey) == null ? true : false;
        }

        public void RemoveObject(string sessionKey)
        {
            _session.Remove(sessionKey);
        }

        public void SetObject(string sessionKey, object sessionObject)
        {
            _session.SetObject(sessionKey, sessionObject);
        }
    }
}
