using System;
using System.Collections.Generic;

#nullable disable

namespace LineBot.Entity.Models
{
    public partial class RentFixedFee
    {
        public int SeqNo { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Fee { get; set; }
    }
}
