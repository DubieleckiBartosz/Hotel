using Application.Features.Hotel.Commands.CreateHotel;
using Application.Features.Hotel.Commands.CreatePictureHotel;
using Application.Features.Hotel.Commands.UpdateHotelData;
using Application.Features.Hotel.Queries.GetHotelByID;
using Application.Features.Hotel.Queries.GetHotelRooms;
using Application.Features.Hotel.Queries.GetHotels;
using Application.Features.Hotel.Queries.GetHotelsByID;
using Application.Features.Room.Commands.CreateRoom;
using Application.Features.Room.Commands.UpdateRoom;
using Application.Features.Room.Queries.GetRoomById;
using Application.Features.Room.Queries.GetRooms;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Helpers;
using Application.Features.Booking.Commands.CreateBooking;
using Application.Features.Booking.Commands.UpdateBooking;
using Application.Features.Booking.Queries.GetBooking;
using Application.Features.File.ModelsToFile;
using Application.Features.Booking.Queries.GetUserBookingIds;

namespace Application.Mapping
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            //Hotel

            CreateMap<CreateHotelCommand, Hotel>()
                .ForMember(c => c.Address, s => s.MapFrom(q =>
                new Address {City=q.City,ZipCode=q.ZipCode,Street=q.Street }));
            CreateMap<Hotel, GetHotelVM>()
                .ForMember(c=>c.Street,s=>s.MapFrom(q=>q.Address.Street))
                .ForMember(c => c.ZipCode, s => s.MapFrom(q => q.Address.ZipCode))
                .ForMember(c => c.City, s => s.MapFrom(q => q.Address.City));
            CreateMap<Hotel, UpdateHotelDto>().ReverseMap();
            CreateMap<Hotel, GetHotelsVM>()
               .ForMember(c => c.City, s => s.MapFrom(q => q.Address.City));
            CreateMap<Hotel, GetHotelsByIdVM>()
                .ForMember(c => c.Street, s => s.MapFrom(q => q.Address.Street))
                .ForMember(c => c.ZipCode, s => s.MapFrom(q => q.Address.Street))
                .ForMember(c => c.City, s => s.MapFrom(q => q.Address.City));

            //HotelRooms
            CreateMap<Hotel, GetHotelWithRoomsVM>()
                .ForMember(c=>c.HotelId,s=>s.MapFrom(q=>q.Id));
      
            CreateMap<Room, RoomInListDto>()
                .ForMember(c => c.IsAvailable, x => x.Ignore())
                .AfterMap((s, r) =>
                {
                    r.IsAvailable = s.GetAvailability();
                });
           
            //PIcture 
            CreateMap<HotelPicture, HotelPictureDto>().ReverseMap();

            //Booking
            CreateMap<CreateBookingCommand, BookingRoom>();
            CreateMap<BookingRoom, UpdateBookingDto>().ReverseMap();
            CreateMap<BookingRoom, GetBookingVM>()
                .ForMember(c => c.PricePerPerson, s => s.MapFrom(q => q.Room.PricePerPerson))
                .ForMember(c => c.FullPrice, s => s.MapFrom(q => q.FullPrice.ToString()));
            CreateMap<BookingRoom, BookingFileVM>();
            CreateMap<BookingRoom, BookingDto>()
                .ForMember(c=>c.BookingId,s=>s.MapFrom(q=>q.Id));

            //Room
            CreateMap<CreateRoomCommand, Room>()
                .ForMember(c=>c.Promotion,s=>s.Ignore());
            CreateMap<UpdateRoomCommand, Room>()
                .ForMember(c => c.PricePerPerson, s => s.MapFrom(q => q.NewPrice));
            CreateMap<Room, GetRoomVM>();
            CreateMap<Room, GetRoomsVM>()
                .ForMember(c => c.IsAvailable, x => x.Ignore())
                .AfterMap((s, r) =>
                {
                    r.IsAvailable = s.GetAvailability();
                });

        }
    }
  
}
