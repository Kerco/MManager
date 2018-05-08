using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Interfaces.Managers
{
    public interface IEnvelopeManager
    {
        Task<List<Envelope>> ListAll();
        Task<List<Envelope>> AccountEnvelope(string id);
        Task<Envelope> Details(int id);
        Task<Envelope> DetailsWithTransaction(int id);
        Task<Envelope> Create(Envelope envelope);
        Task<Envelope> Edit(int id, Envelope envelope);
        Task Delete(int id);



    }
}
