using Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class BookingRepository:BaseRepository<BookingRoom>,IBookingRepository
    {
        public BookingRepository(ApplicationDbContext db):base(db)
        {

        }

        public async Task<BookingRoom> GetBookingByIdAsync(Guid id) =>
            await FindByCondition(c => c.Id == id).Include(c => c.Room)
            .ThenInclude(c=>c.Hotel)
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<BookingRoom>> GetBookingsAsync(DateTime dateTime) =>
                          await FindByCondition(c => c.Created > dateTime).ToListAsync();
    }
}
