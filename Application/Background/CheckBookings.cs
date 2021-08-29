using Application.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Background
{
    //public class CheckBookings : BackgroundService
    //{
    //    private readonly IServiceProvider _serviceProvider;
    //    public CheckBookings(IServiceProvider serviceProvider)
    //    {
    //        _serviceProvider = serviceProvider;
    //    }
    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {

    //        while (!stoppingToken.IsCancellationRequested)
    //        {
    //            await DoWork();
    //            await Task.Delay(TimeSpan.FromDays(1),stoppingToken);
    //        }
    //    }
    //    private async Task DoWork()
    //    {
    //        using var scope = _serviceProvider.CreateScope();
    //        var repoBooking = scope.ServiceProvider.GetRequiredService<IBookingRepository>();
    //        //ToDO=> Data w zależności od ustwaień zamieszczonych przy tworzeniu hotelu...
    //        var result =await repoBooking.GetBookingsAsync(DateTime.Now.AddDays(-1));
    //        if (result.Any())
    //        {

    //        }
    //    }
    //}
}
