using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Queries.GetHotels
{
    public class GetHotelsVM
    {
        public Guid Id { get; set; }
        public string HotelName { get; set; }
        public int Stars { get; set; }
        public string City { get; set; }
    }
}
