using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class Attachment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
    public class HotelAttachment : Attachment
    {
        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
    public class RoomAttachment : Attachment 
    {
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
    }
}
