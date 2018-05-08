using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    public class Transaction
    {
        public int Id { get; set; }
        public int EnvelopeId { get; set; }
        public Envelope Envelope { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public int Value { get; set; }
        public DateTime Date { get; set; }
        public bool Type { get; set; }



    }
}
