using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    public class Envelope
    {
        public Envelope()
        {
            Transactions = new List<Transaction>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string AccountId { get; set; }
        public int Value { get; set; }
        public string Details { get; set; }
        public Account Account { get; set; }
        public ICollection<Transaction> Transactions { get; set; }



    }
}
