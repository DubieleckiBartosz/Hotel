using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.File.CSV.Queries.GetBookingsOfRooms
{
    public class GetBookingsOfRoomsCommand :GetCSV,IRequest<FileVM>
    {
    }

}
