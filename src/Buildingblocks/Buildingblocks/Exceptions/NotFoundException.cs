
using System.Net;

namespace Buildingblocks.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message , object key ) : base(message,HttpStatusCode.NotFound)
        {
            
        }
    }
}
