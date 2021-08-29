using Application.Features.Booking.Commands.CreateBooking;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class GetBookingPrice
    {
        public static decimal GetFullPriceBooking(this Room room,int days,string promotion)
        {
            var result = Math.Round((room.NumberOfBeds* room.PricePerPerson) * days, 2);
           
            if (promotion != null)
            {
             var pro= promotion.Remove(promotion.Length-1);
              if(decimal.TryParse(pro,out decimal value))
                {
                    return result-((value/100)* result);
                }
            }
            return result;
        }
        public static string CheckDiscount(this string discount)
        {
            if(discount!= null)
            {
                decimal.TryParse(discount, out decimal disc);
                if(disc == 0)
                {
                    return "There is no discount";
                }
                return disc.ToString();
            }
            return "There is no discount"; 
        }
    }
}
