﻿using System;
using System.Collections.Generic;

namespace AZSync.API.Models
{
    public partial class Users
    {
        public long Id { get; set; }
        public string Name { get; set; }
        //public string Address { get; set; }
        public string Email { get; set; }
    }
}