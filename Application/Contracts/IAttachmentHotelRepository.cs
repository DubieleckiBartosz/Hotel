using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAttachmentHotelRepository:IBaseRepository<HotelAttachment>
    {
        Task<HotelAttachment> GetByIdAsync(Guid id);
        Task<IEnumerable<HotelAttachment>> GetByHotelIdAsync(Guid id);
    }
}
