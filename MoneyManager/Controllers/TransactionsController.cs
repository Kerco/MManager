using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Data;
using MoneyManager.Interfaces.Managers;
using MoneyManager.Models;

namespace MoneyManager.Controllers
{
    [Route("Transactions/[Action]")]
    public class TransactionsController : Controller
    {
        private ITransactionManager TransactionManager { get; }

        public ManagerContext _context;
        public TransactionsController(ManagerContext context, ITransactionManager transactionManager)
        {
            _context = context;
            TransactionManager = transactionManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var trans = await TransactionManager.ListAllAsync();
            return Ok(trans);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var transaction = await TransactionManager.Details(id);
            return Ok(transaction);
        }

        [HttpGet]
        public async Task<IActionResult> EnvelopeTransaction(int id)
        {
            var transaction = await TransactionManager.EnvelopeTransaction(id);
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                await TransactionManager.Create(transaction);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var transaction = await TransactionManager.Details(id);
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> EditTransaction(int id, [FromBody] Transaction transaction)
        {

            if (ModelState.IsValid)
            {
                await TransactionManager.Edit(id, transaction);
                return Ok();
            }
            return BadRequest();
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await TransactionManager.Details(id);
            return Ok(transaction);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await TransactionManager.Delete(id);
            return Ok();
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}