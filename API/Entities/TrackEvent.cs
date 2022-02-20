using System;

namespace API.Entities
{
    public class TrackEvent
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int LocationId { get; set; }
        public Location SiteLocation { get; set; }
        public int? AgentId { get; set; }
        public User Agent { get; set; }
        public string Status { get; set; }
        public bool Done { get; set; }
    }
}
