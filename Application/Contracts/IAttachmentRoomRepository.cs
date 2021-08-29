using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAttachmentRoomRepository:IBaseRepository<RoomAttachment>
    {
        Task<RoomAttachment> GetByIdAsync(Guid id);
        Task<IEnumerable<RoomAttachment>> GetByRoomIdAsync(Guid id);
    }
}
