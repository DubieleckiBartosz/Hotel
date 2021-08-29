using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Helpers
{
    public class PagingHelper<T>
    {
        public static async Task<(List<T>,int)> GetPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var result = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (result, count);
        }
    }
}
