using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenBasedAuth_NetCore.Utilities
{
    public interface ISessionService
    {
        void SetObject(string sessionKey, object sessionObject);
        T GetObject<T>(string sessionKey);

        void RemoveObject(string sessionKey);

        bool IsNull(string sessionKey);


    }
}
