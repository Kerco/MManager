using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int EnvelopeId { get; set; }
        public Envelope Envelope { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public int Value { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public bool Type { get; set; }



    }
}
