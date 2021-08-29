using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Commands.DeleteHotel
{
    public class DeleteHotelCommand:IRequest<Response<string>>
    {
        public Guid HotelId { get; set; }
    }
}
