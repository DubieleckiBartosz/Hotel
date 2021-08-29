using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Queries.GetHotelByID
{
    public class GetHotelQuery:IRequest<Response<GetHotelVM>>
    {
        public Guid HotelId { get; set; }
    }
}
