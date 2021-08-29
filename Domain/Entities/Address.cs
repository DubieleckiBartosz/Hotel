using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public Hotel Hotel { get; set; }
        public Address() { }
        public Address(string street, string zipcode, string city)
        {
            Street = street;
            ZipCode = zipcode;
            City = city;
        }
    }
}
