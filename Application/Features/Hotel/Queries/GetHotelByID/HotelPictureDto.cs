using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Queries.GetHotelByID
{
    public class HotelPictureDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Main { get; set; }
        public byte[] Image { get; set; }
    }
}
