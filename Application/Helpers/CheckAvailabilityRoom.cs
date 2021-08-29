using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class CheckAvailabilityRoom
    {
        public static bool GetAvailability(this Room room)
        {
            var dateToCalculate = DateTime.UtcNow;
            var availableResult = room.BookingRooms.Any(c =>
            (dateToCalculate > c.From)
            && (dateToCalculate < c.To));
            return  (availableResult != false)
            ? false : true;
        }
        public static bool GetAvailabilityByDate(this Room room,DateTime from,DateTime to)
        {
            var exist = room.BookingRooms.Any(c =>
          (from >= c.From && from <= c.To) || (to <= c.To && to >= c.From));
            return exist is true ? false : true;
        }
    }
}
