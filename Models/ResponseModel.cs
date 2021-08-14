using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TokenBasedAuth_NetCore.Models
{
    public class ResponseModel<T>  where T : class
    {
        public HttpStatusCode Status { get; set; }

        public string Message { get; set; }

        public T Result { get; set; } 

    }
}
