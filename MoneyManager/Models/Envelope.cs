using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class Envelope
    {
        public Envelope()
        {
            Transactions = new List<Transaction>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string AccountId { get; set; }
        public int Value { get; set; }
        public string Details { get; set; }
        public Account Account { get; set; }
        public ICollection<Transaction> Transactions { get; set; }



    }
}
