using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions.Custom_Exceptions
{
    public class HttpFetchRequestException : CustomException
    {
        public HttpFetchRequestException(string message) : 
            base(message, null, HttpStatusCode.InternalServerError)
        {
        }
    }
}
