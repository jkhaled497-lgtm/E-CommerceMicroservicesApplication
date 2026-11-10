using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Buildingblocks.Exceptions
{
    public class BadRequestException : AppException
    {
        public IDictionary<string,string[]>? Errors { get; set; }

        public BadRequestException(IDictionary<string, string[]> errors) : base("One or more validation exception occured" , HttpStatusCode.BadRequest)
        {
            Errors = errors;

        }
        public BadRequestException(string filed , string message ):base("One or more validation exception occured" , HttpStatusCode.BadRequest)
        {
            Errors = new Dictionary<string, string[]>
            {
                { filed, new[] { message } }
            };
        }

        public BadRequestException(IDictionary<string,string> errors) : base("One or more validation exception occured", HttpStatusCode.BadRequest)
        {
            Errors = new Dictionary<string, string[]>
            {
                {   errors.Keys.First(), new[] { errors.Values.First() } }
            };
        }
    }
}
