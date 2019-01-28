using System;
using System.Collections.Generic;

namespace AZSync.API.Models
{
    public partial class Address
    {
        public long Id { get; set; }
        public string Addressline1 { get; set; }
        public string Addressline2 { get; set; }
        public string Addressline3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public long Zip { get; set; }
        public long? MemberId { get; set; }

        public Member Member { get; set; }
    }
}
