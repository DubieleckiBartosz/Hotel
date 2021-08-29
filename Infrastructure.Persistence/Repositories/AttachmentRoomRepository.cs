using Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class AttachmentRoomRepository:BaseRepository<RoomAttachment>, IAttachmentRoomRepository
    {
        public AttachmentRoomRepository(ApplicationDbContext db):base(db)
        {

        }

        public async Task<RoomAttachment> GetByIdAsync(Guid id) =>
            await FindByCondition(c => c.Id == id).FirstOrDefaultAsync();


        public async Task<IEnumerable<RoomAttachment>> GetByRoomIdAsync(Guid id) =>
            await FindByCondition(c => c.RoomId == id).ToListAsync();

    }
}
