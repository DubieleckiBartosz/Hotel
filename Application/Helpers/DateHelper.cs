using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class DateHelper
    {
        public static DateTime? GetDateTime(this string date)
        {
            if(date is not null)
            {
                DateTime.TryParse(date, out DateTime myDate);
                return myDate;
            }
            return default;
        }
    }
}
