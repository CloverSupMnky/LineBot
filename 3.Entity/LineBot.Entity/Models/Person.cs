using System;
using System.Collections.Generic;

#nullable disable

namespace LineBot.Entity.Models
{
    public partial class Person
    {
        public Guid PersonId { get; set; }
        public string PersonName { get; set; }
    }
}
