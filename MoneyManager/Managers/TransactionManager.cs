using Microsoft.EntityFrameworkCore;
using MoneyManager.Data;
using MoneyManager.Interfaces.Managers;
using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Managers
{
    public class TransactionManager : ITransactionManager
    {

        private ManagerContext _context { get; }

        public TransactionManager(ManagerContext Context)
        {
            _context = Context;
        }

        public async Task<Transaction> Create(Transaction transaction)
        {
            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task Delete(int id)
        {
            var transaction = await _context.Transactions
                        .SingleOrDefaultAsync(t => t.Id == id);
            if (transaction == null)
            {
                //return NotFound();
            }
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction> Details(int id)
        {
            var transaction = await _context.Transactions
                   .SingleOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                //throw not found exception...
            }
            return transaction;
        }

        public async Task<Transaction> Edit(int id, Transaction transaction)
        {
            if (id != transaction.Id)
            {
                //return not found
            }
            _context.Update(transaction);
            await _context.SaveChangesAsync();

            if (!TransactionExists(transaction.Id))
            {
                //return NotFound();
            }
            return transaction;

        }


        public async Task<List<Transaction>> EnvelopeTransaction(int id)
        {
            var transaction = await _context.Transactions
                                            .Where(t => t.EnvelopeId == id).ToListAsync();
            if (transaction == null)
            {
                //return NotFound();
            }
            return transaction;
        }

        public async Task<List<Transaction>> ListAllAsync()
        {
            var transaction = await _context.Transactions.ToListAsync();
            return transaction;
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }

    }
}
