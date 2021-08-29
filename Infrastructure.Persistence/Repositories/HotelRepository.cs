using Application.Contracts;
using Dapper;
using Domain.Entities;
using Infrastructure.Persistence.Database;
using Infrastructure.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        public HotelRepository(ApplicationDbContext db)
            : base(db)
        {
        }

        public async Task<(IEnumerable<Hotel>,int)> FindAllHotelsAsync(int pageNumber, int pageSize, string searchPhrase,int stars)
        {
            var collection = FindAll().Include(c => c.Address) as IQueryable<Hotel>;
            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                var searchQuery = searchPhrase.Trim();
                collection = collection.Where(c => c.HotelName.Contains(searchQuery) 
                || c.Address.Street.Contains(searchQuery)
                || c.Address.City.Contains(searchQuery) || c.Email.Contains(searchQuery));
            }
            if(stars!=0)
            {
                collection = collection.Where(c => c.Stars == stars);
            }
            return await PagingHelper<Hotel>.GetPagedList(collection, pageNumber, pageSize);
        }
            
        public async Task<Hotel> GetHotelByIdAsync(Guid id) =>
           await FindByCondition(c => c.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Hotel>> GetHotelsByIdAsync(IEnumerable<Guid> ids) =>
            await FindByCondition(c => ids.Contains(c.Id))
            .Include(c=>c.Address).ToListAsync();

        public async Task<Hotel> GetHotelWithDetailsByIdAsync(Guid id)
        {
            return await FindByCondition(c => c.Id.Equals(id))
                .Include(s => s.Address)
                .Include(c=>c.HotelPictures)
                .FirstOrDefaultAsync();
        }

        public async Task<Hotel> GetHotelWithRoomsAsync(Guid id) =>
            await FindByCondition(c => c.Id == id)
            .Include(s => s.Rooms)
            .ThenInclude(q => q.BookingRooms)
            .FirstOrDefaultAsync();

        public async Task<Hotel> GetbookingsHotelForUserAsync(string userId, Guid id)
        {
            var collection = FindByCondition(c => c.Id == id);
            if (collection is not null)
            {
                collection = collection.Include(c => c.Rooms)
                    .ThenInclude(c => c.BookingRooms.Where(s => s.UserId == userId));
                return await collection.FirstOrDefaultAsync();
            }
            return null;
        }
    }
}
