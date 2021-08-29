using FluentValidation;

namespace Application.Features.Hotel.Commands.CreatePictureHotel
{
    public class CreateHotelPictureValidator:AbstractValidator<CreateHotelPictureCommand>
    {
        public CreateHotelPictureValidator()
        {
            RuleFor(c => c.Name).MaximumLength(100);
            RuleFor(c => c.File).NotEmpty();
        }
    }
}
