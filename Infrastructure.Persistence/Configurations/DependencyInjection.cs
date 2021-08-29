using Application.Contracts;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection InfrastructureDependency(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IHotelPictureRepository, HotelPictureRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IAttachmentHotelRepository, AttachmentHotelRepository>();
            services.AddScoped<IAttachmentRoomRepository, AttachmentRoomRepository>();
            return services;
        }
        //public static IServiceCollection GetDapper(this IServiceCollection services) =>
        //    services.AddSingleton<DapperContext>();
    }
}
