using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IRoomRepository:IBaseRepository<Room>
    {
        Task<Room> GetRoomByIdAsync(Guid hotelId, Guid roomId);
        Task<Room> GetRoomWithBookingsAsync(Guid hotelId, Guid roomId);
        Task<IEnumerable<Room>> GetRoomsWithAvailabilityAsync(Guid hotelId);
        Task<IEnumerable<Room>> GetRoomsByParametersAsync(Guid hotelId, DateTime? start, DateTime? end);
        Task<Room> GetRoomByParametersAsync(Guid hotelId, Guid roomId, DateTime? start, DateTime? end);
    }
}
