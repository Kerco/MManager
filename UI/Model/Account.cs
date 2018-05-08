using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    public class Account
    {
        public Account()
        {
            Envelopes = new List<Envelope>();
        }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public bool Gender { get; set; }
        public DateTime BirthDay { get; set; }

        public ICollection<Envelope> Envelopes { get; set; }
    }
}
