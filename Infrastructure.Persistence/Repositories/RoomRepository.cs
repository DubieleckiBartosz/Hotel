using Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class RoomRepository:BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext db):base(db)
        {

        }

        public async Task<Room> GetRoomByParametersAsync(Guid hotelId, Guid roomId, DateTime? start, DateTime? end)
        {
            var date = DateTime.UtcNow;
            var dateFrom = date.AddMonths(-1);

            var result = await FindByCondition(c => c.HotelId == hotelId && c.Id == roomId)
           .Include(c => c.BookingRooms.Where(s =>
           (start == null && end == null) ? s.From.Date >= dateFrom : (end != null && start != null)
           ? (s.From.Date >= start && s.To.Date <= end)
           : (end == null && start != null) ? (s.From.Date >= start) : s.To <= end)).FirstOrDefaultAsync();

            return result;
        }

        public async Task<Room> GetRoomByIdAsync(Guid hotelId, Guid roomId) =>
            await FindByCondition(c => c.HotelId == hotelId
            && c.Id == roomId).Include(c=>c.Hotel).FirstOrDefaultAsync();

        public async Task<IEnumerable<Room>> GetRoomsWithAvailabilityAsync(Guid hotelId) =>
            await FindByCondition(c => c.HotelId == hotelId)
            .Include(c => c.BookingRooms.Where(c=>c.From.Date>=DateTime.UtcNow.AddMonths(-6)))
            .ToListAsync();
      

        public async Task<Room> GetRoomWithBookingsAsync(Guid hotelId, Guid roomId) =>
            await FindByCondition(c => c.HotelId == hotelId && c.Id== roomId)
            .Include(c => c.BookingRooms).FirstOrDefaultAsync();

        public async Task<IEnumerable<Room>> GetRoomsByParametersAsync(Guid hotelId, DateTime? start, DateTime? end)
        {
            var date = DateTime.UtcNow;
            var dateFrom = date.AddMonths(-1);

            var result = await FindByCondition(c => c.HotelId == hotelId)
           .Include(c => c.BookingRooms.Where(s =>
           (start == null && end == null) ? s.From.Date >= dateFrom : (end != null && start != null)
           ? (s.From.Date >= start && s.To.Date <= end)
           : (end == null && start != null) ? (s.From.Date >= start) : s.To <= end))
            .ToListAsync();

            return result;
        }
    }
}
