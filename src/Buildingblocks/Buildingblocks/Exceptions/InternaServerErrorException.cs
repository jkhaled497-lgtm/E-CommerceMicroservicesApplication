using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Buildingblocks.Exceptions
{
    public class InternaServerErrorException : AppException
    {
        public InternaServerErrorException(string message , string detials) : base(message,HttpStatusCode.InternalServerError)
        {
            
        }

        public string? Details { get; set; }
    }
}
