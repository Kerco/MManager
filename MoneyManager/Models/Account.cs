using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class Account : IdentityUser
    {
        public Account()
        {
            Envelopes = new List<Envelope>();
        }
        //public int Id { get; set; }
        //[EmailAddress]
        //public string Email { get; set; }
        //public string Password { get; set; }
        //public string UserName { get; set; }
        public bool Gender { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDay { get; set; }

        public ICollection<Envelope> Envelopes { get; set; }
    }
}
