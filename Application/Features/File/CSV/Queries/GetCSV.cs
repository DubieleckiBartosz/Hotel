using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;


namespace Application.Features.File.CSV.Queries
{
    public abstract class GetCSV
    {
        [BindNever]
        public string UserId { get; set; }
        public Guid HotelId { get; set; }
        public string FileName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
 

}
