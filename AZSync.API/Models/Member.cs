using System;
using System.Collections.Generic;

namespace AZSync.API.Models
{
    public partial class Member
    {
        public Member()
        {
            Address = new HashSet<Address>();
            Contact = new HashSet<Contact>();
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ICollection<Address> Address { get; set; }
        public ICollection<Contact> Contact { get; set; }
    }
}
