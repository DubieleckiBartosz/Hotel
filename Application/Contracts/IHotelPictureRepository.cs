using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IHotelPictureRepository:IBaseRepository<HotelPicture>
    {
        Task<IEnumerable<HotelPicture>> GetPicturesByHotelIdAsync(Guid id); 

    }

}
