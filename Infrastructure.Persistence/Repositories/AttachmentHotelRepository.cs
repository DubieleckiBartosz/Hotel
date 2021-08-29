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
    public class AttachmentHotelRepository:BaseRepository<HotelAttachment>,IAttachmentHotelRepository
    {
        public AttachmentHotelRepository(ApplicationDbContext db):base(db)
        {

        }

        public async Task<IEnumerable<HotelAttachment>> GetByHotelIdAsync(Guid id) =>
            await FindByCondition(c => c.HotelId == id).ToListAsync();


        public async Task<HotelAttachment> GetByIdAsync(Guid id) =>
            await FindByCondition(c => c.Id == id).FirstOrDefaultAsync();


    }
}
