using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FileContentTypeAttribute:Attribute
    {
        private readonly string _contentType;
        public FileContentTypeAttribute(string contentType)
        {
            _contentType = contentType;
        }
    }
}
