using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Buildingblocks.Exceptions
{
    public class AppException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public Dictionary<string,string[]> Errors { get; set; }

        public AppException(string message , HttpStatusCode httpStatus):base(message)
        {
            HttpStatusCode = httpStatus;
        }
    }
}
