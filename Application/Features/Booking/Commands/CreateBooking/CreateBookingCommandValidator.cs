using Application.Contracts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Booking.Commands.CreateBooking
{
    public class CreateBookingCommandValidator:AbstractValidator<CreateBookingCommand>
    {
        
        public CreateBookingCommandValidator()
        {
            RuleFor(c => c.Email).EmailAddress().NotEmpty();
            RuleFor(c => c.To).Must((c, cancellation) =>
              {
                  if (c.To < DateTime.UtcNow.Date 
                        || c.From<DateTime.UtcNow.Date)
                  {
                      return false;
                  }
                  if(c.To > c.From)
                  {
                      return true;
                  }
                  
                  return false;
              }).NotEmpty().WithMessage("Check the entered dates");
            RuleFor(c => c.From).NotEmpty();
        }
    }
}
