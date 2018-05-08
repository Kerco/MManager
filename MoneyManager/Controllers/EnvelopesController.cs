using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Filter;
using MoneyManager.Interfaces.Managers;
using MoneyManager.Models;

namespace MoneyManager.Controllers
{
    [Route("Envelopes/[Action]")]
    public class EnvelopesController : Controller
    {
        private IEnvelopeManager Envelopemanager { get; }

        public EnvelopesController(IEnvelopeManager envelopeManager)
        {
            Envelopemanager = envelopeManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var envelopes = await Envelopemanager.ListAll();
            return Ok(envelopes);
        }



        [HttpGet]
        public async Task<IActionResult> AccountEnvelope(string id)
        {
            var envelopes = await Envelopemanager.AccountEnvelope(id);
            return Ok(envelopes);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var envelope = await Envelopemanager.Details(id);
            return Ok(envelope);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsWithTransaction(int id)
        {
            var envelope = await Envelopemanager.DetailsWithTransaction(id);
            return Ok(envelope);
        }


        [HttpPost]
        [ModelStateFilter]
        public async Task<IActionResult> Create([FromBody] Envelope envelope)
        {
            await Envelopemanager.Create(envelope);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var envelope = await Envelopemanager.Details(id);
            return Ok(envelope);
        }
        [HttpPost]
        [ModelStateFilter]
        public async Task<IActionResult> EditEnvelope(int id, [FromBody] Envelope envelope)
        {
            await Envelopemanager.Edit(id, envelope);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var envelope = await Envelopemanager.Details(id);
            return Ok(envelope);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await Envelopemanager.Delete(id);
            return Ok();
        }

    }
}