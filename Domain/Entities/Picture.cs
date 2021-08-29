using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class Picture
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Main { get; set; }
        public byte[] Image { get; set; }
    }
    public class HotelPicture: Picture
    { 
        public Guid HotelId{ get; set; }
        public Hotel Hotel { get; set; }
    }

    public class RoomPicture : Picture 
    {
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
    }
}
