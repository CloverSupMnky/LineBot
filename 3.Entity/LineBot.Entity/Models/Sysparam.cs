using System;
using System.Collections.Generic;

#nullable disable

namespace LineBot.Entity.Models
{
    public partial class Sysparam
    {
        public string GroupId { get; set; }
        public string ItemId { get; set; }
        public string ItemValue { get; set; }
        public string Description { get; set; }
    }
}
