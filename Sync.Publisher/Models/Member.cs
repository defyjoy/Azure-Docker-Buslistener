using System;
using System.Collections.Generic;
using System.Text;

namespace Sync.Publisher.Models
{
    [Serializable]
    public class Member
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public List<Contact> Contacts { get; set; }
    }
}
