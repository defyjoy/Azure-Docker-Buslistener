using System;
using System.Collections.Generic;

namespace AZSync.API.Models
{
    public partial class Contact
    {
        public long Id { get; set; }
        public long MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Member Member { get; set; }
    }
}
