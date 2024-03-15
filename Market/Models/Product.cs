﻿using Market.Models.Base;

namespace Market.Models
{
    public class Product : BaseModel
    {
        public Guid? CategoryId { get; set; }
        public decimal? Price { get; set; }
        public virtual Category? Category { get; set; }
        public virtual List<Storage> Storages { get; set; } = new List<Storage>();
    }
}
