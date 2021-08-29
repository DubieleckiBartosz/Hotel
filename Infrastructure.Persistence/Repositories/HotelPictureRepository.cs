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
    public class HotelPictureRepository:BaseRepository<HotelPicture>,IHotelPictureRepository
    {
        public HotelPictureRepository(ApplicationDbContext db):base(db)
        {

        }

        public async Task<IEnumerable<HotelPicture>> GetPicturesByHotelIdAsync(Guid id) =>
            await FindByCondition(c => c.HotelId == id).ToListAsync();

    }
}
