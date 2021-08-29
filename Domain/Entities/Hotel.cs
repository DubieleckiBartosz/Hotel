using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Hotel: AuditableEntity
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string HotelName { get; set; }
        public int Stars { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<HotelPicture> HotelPictures { get; set; }
        public virtual ICollection<HotelAttachment> HotelAttachments { get; set; }
        public Hotel() { }

        public Hotel(string name, Guid addressId, string email, int stars = 0)
        {
            HotelName = name;
            Stars = stars;
            AddressId = addressId;
            Email = email;
        }
    }
}
