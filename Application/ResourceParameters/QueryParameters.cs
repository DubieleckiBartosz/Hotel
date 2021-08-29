using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ResourceParameters
{
    public abstract class QueryParameters
    {
        private const int _maxSize=50;
        private int _pageSize=10;
        public int PageNumber { get; set; } = 1;
        public string SearchQuery { get; set; }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > _maxSize) ? _maxSize : value; 
        }

    }
    public class RoomQueryParameters : QueryParameters
    {

    }
    public class HotelQueryParameters: QueryParameters
    {
        private const int _maxStars = 5;
        private int _stars;
        public int Stars
        {
            get => _stars;
            set => _stars = (value > _maxStars) ? _maxStars : value;
        }
    }
}
