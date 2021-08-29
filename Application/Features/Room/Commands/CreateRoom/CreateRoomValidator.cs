using Application.Contracts;
using Application.Exceptions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Room.Commands.CreateRoom
{
    public class CreateRoomValidator : AbstractValidator<CreateRoomCommand>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserContextService _userContextService;
        public CreateRoomValidator(IHotelRepository hotelRepository,
            IUserContextService userContextService)
        {
            _userContextService = userContextService;
            _hotelRepository = hotelRepository;
            RuleFor(c => c.NumberOfBeds).Must((c) =>
            {
                return c > 0 && c <= 20;
            });
            RuleFor(c => c).MustAsync(async (c, cancellation) => 
            {
                (bool exist,int stars) = await HotelStars(c.HotelId);
                if (!exist)
                {
                    return false;
                }
                if (stars < 3 && c.PricePerPerson>500)
                {
                    return false;
                }
                return true;
            });
            RuleFor(c => c.Promotion)
                .GreaterThanOrEqualTo(0)
                .LessThan(100);
        }

        private async Task<(bool,int)> HotelStars(Guid id)
        {
            var hotel = await _hotelRepository.GetHotelByIdAsync(id);
            var userId = _userContextService.UserId;
            if (hotel.UserId != userId && _userContextService.IsAdmin())
            {
                return (false, 0);
            }
            if (hotel is null)
            {
                return (false, 0);
            }   
            return (true,hotel.Stars);
        }
    }
}
