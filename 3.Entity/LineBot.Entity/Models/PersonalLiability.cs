using System;
using System.Collections.Generic;

#nullable disable

namespace LineBot.Entity.Models
{
    public partial class PersonalLiability
    {
        public int SeqNo { get; set; }
        public Guid CreditorId { get; set; }
        public Guid DebtorId { get; set; }
        public string Description { get; set; }
        public decimal Fee { get; set; }
        public int CreateOn { get; set; }
        public bool IsClosed { get; set; }
        public int? ClosedOn { get; set; }
    }
}
