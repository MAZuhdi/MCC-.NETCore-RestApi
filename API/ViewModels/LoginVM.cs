﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.ViewModels
{
    public class LoginVM
    {
        [Required][EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
