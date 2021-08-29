using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Room.Queries.GetRoomById
{
    public class GetRoomVM
    {
        public Guid Id { get; set; }
        public int NumberOfBeds { get; set; }
        public decimal PricePerPerson { get; set; }
        public bool IsAvailable { get; set; }
    }
}
