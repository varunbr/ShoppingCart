using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class User : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public DateTime LastActive { get; set; }
        public string Name { get; set; }
        public int? PhotoId { get; set; }
        public Photo Photo { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int? AddressId { get; set; }
        public Address Address { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<TrackEvent> TrackEvents { get; set; }
        public ICollection<TrackAgent> TrackAgents { get; set; }
    }
}
