using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Queries.GetHotelByID
{
    public class GetHotelVM
    {
        public Guid Id { get; set; }
        public string HotelName { get; set; }
        public int Stars { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public IEnumerable<HotelPictureDto> HotelPictures { get; set; }
    }
}
