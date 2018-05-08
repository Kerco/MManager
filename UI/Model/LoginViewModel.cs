﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    class LoginViewModel
    {
        [Required]
        // [EmailAddress]
        public string Email { get; set; }
        // public Account Account { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
