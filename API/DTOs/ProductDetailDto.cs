﻿using System.Collections.Generic;

namespace API.DTOs
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public string Features { get; set; }
        public double Amount { get; set; }
        public string Category { get; set; }
        public bool Available { get; set; }
        public double DeliveryCharge { get; set; }
        public string StoreName { get; set; }
        public int StoreItemId { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
        public ICollection<PropertyValueDto> Properties { get; set; }
    }
}
