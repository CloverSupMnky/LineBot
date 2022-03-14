using System;
using System.Collections.Generic;

#nullable disable

namespace LineBot.Entity.Models
{
    public partial class UtilityFee
    {
        public int SeqNo { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Fee { get; set; }
        public int CreateOn { get; set; }
        public int? ClosedOn { get; set; }
        public bool IsClosed { get; set; }
    }
}
