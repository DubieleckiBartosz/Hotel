using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Commands.UpdateHotelData
{
    public class UpdateHotelValidator:AbstractValidator<UpdateHotelDto>
    {
        public UpdateHotelValidator()
        {
            RuleFor(c => c.HotelName).Length(5,100);
            RuleFor(c => c.Email).EmailAddress().MaximumLength(50);
        }
    }
}
