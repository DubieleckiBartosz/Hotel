using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IBookingRepository:IBaseRepository<BookingRoom>
    {
        Task<BookingRoom> GetBookingByIdAsync(Guid id);
        Task<IEnumerable<BookingRoom>> GetBookingsAsync(DateTime dateTime);
    }
}
