using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Queries.GetHotelsByID
{
    public class GetHotelsByIdQuery:IRequest<IEnumerable<GetHotelsByIdVM>>
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}
