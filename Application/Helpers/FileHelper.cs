using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class FileHelper
    {

        public static byte[] GetContentBytes(this IFormFile file)
        {
            using(var ms=new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }
        public static byte[] GetContentToTXT<T>(this IEnumerable<T> bookings)
        {
            StringBuilder sb = new StringBuilder();
            var properties = typeof(T).GetProperties();
            foreach (var item in bookings)
            {
                foreach (var prop in properties)
                {
                    object propValue = prop.GetValue(item, null);
                    sb.AppendLine($"{prop.Name}:  {propValue}");
                }
                sb.Append("\n\n");
            }
            return ASCIIEncoding.ASCII.GetBytes(sb.ToString());
        }

        public static string SaveFile(this IFormFile file)
        {
          const string root= @"C:\Users\bdubi\Desktop\RestApi\FewPhotos_for_Programs";
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
            string path = Path.Combine(root, file.FileName);
            using (Stream stream=new FileStream(path,FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return path;
        }
    }
}
