using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Hotel.Commands.CreateHotel
{
    public class CreateHotelCommandValidator:AbstractValidator<CreateHotelCommand>
    {
        public CreateHotelCommandValidator()
        {
            RuleFor(c => c.HotelName).MaximumLength(100).NotEmpty();
            RuleFor(c => c.Email).NotEmpty().EmailAddress().MaximumLength(50);
            RuleFor(c => c.Stars).LessThanOrEqualTo(5);
            RuleFor(c => c.Street).Length(5,100).NotEmpty();
            RuleFor(c => c.UserId).NotEmpty().MaximumLength(450);
            RuleFor(c => c.PhoneNumber).NotEmpty();
            RuleFor(c => c.ZipCode).Length(5,10).NotEmpty();
            RuleFor(c => c.City).Length(3, 50).NotEmpty();
        }
    }
}
