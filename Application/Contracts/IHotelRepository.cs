using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IHotelRepository:IBaseRepository<Hotel>
    {
        Task<Hotel> GetHotelWithDetailsByIdAsync(Guid id);
        Task<Hotel> GetHotelByIdAsync(Guid id);
        Task<IEnumerable<Hotel>> GetHotelsByIdAsync(IEnumerable<Guid> ids);
        Task<(IEnumerable<Hotel>, int)> FindAllHotelsAsync(int pageNumber, int pageSize, string searchPhrase,int stars);
        Task<Hotel> GetHotelWithRoomsAsync(Guid id);
        Task<Hotel> GetbookingsHotelForUserAsync(string userId, Guid id);
    }
}
