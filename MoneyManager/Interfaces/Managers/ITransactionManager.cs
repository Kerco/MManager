using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Interfaces.Managers
{
    public interface ITransactionManager
    {
        Task<List<Transaction>> ListAllAsync();
        Task<Transaction> Details(int id);
        Task<List<Transaction>> EnvelopeTransaction(int id);
        Task<Transaction> Create(Transaction transaction);
        Task<Transaction> Edit(int id, Transaction transaction);
        Task Delete(int id);
    }
}
