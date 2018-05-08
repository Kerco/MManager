using Microsoft.EntityFrameworkCore;
using MoneyManager.Data;
using MoneyManager.Exceptions;
using MoneyManager.Filter;
using MoneyManager.Interfaces.Managers;
using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Managers
{
    
    public class EnvelopeManager : IEnvelopeManager
    {
        private ManagerContext _context { get; }

        public EnvelopeManager(ManagerContext Context)
        {
            _context = Context;
        }
        public async Task<List<Envelope>> ListAll()
        {
            var envelopes = await _context.Envelopes.ToListAsync();
            return envelopes;
        }
        
        [NotFoundExceptionFilter]
        public async Task<List<Envelope>> AccountEnvelope(string id)
        {
            if (id == null)
            {
                throw new NotFoundException();
            }
            var envelopes = await _context.Envelopes.Where(e => e.Account.Id == id).ToListAsync();

            return envelopes;
        }

        public async Task<Envelope> Details(int id)
        {
            var envelope = await _context.Envelopes
                                         .SingleOrDefaultAsync(m => m.Id == id);
            if (envelope == null)
            {
                //return NotFound();
            }
            return envelope;
        }

        public async Task<Envelope> DetailsWithTransaction(int id)
        {
            var envelope = await _context.Envelopes.Include(e => e.Transactions)
                                   .SingleOrDefaultAsync(m => m.Id == id);

            if (envelope == null)
            {
                //return NotFound();
            }
            return envelope;
        }

        public async Task<Envelope> Create(Envelope envelope)
        {
            _context.Add(envelope);
            await _context.SaveChangesAsync();
            return envelope;
        }

        public async Task<Envelope> Edit(int id, Envelope envelope)
        {
            if (id != envelope.Id)
            {
                throw new NotFoundException();
            }
            _context.Update(envelope);
            await _context.SaveChangesAsync();

            if (!EnvelopeExists(envelope.Id))
            {
                throw new NotFoundException();
            }
            return envelope;
        }

        public async Task Delete(int id)
        {
            var envelope = await _context.Envelopes.SingleOrDefaultAsync(e => e.Id == id);

            if (envelope == null)
            {
                //return NotFound();
            }

            _context.Envelopes.Remove(envelope);
            await _context.SaveChangesAsync();
        }

        private bool EnvelopeExists(int id)
        {
            return _context.Envelopes.Any(e => e.Id == id);
        }
    }
}
