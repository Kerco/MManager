using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UI.Exceptions;
using UI.Model;

namespace UI.Services
{
    class EnvelopeManager
    {
        private readonly Uri baseAddr = new Uri("http://localhost:55388/");
        /// <summary>
        /// Returns all Envelopes from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Envelope>> ListEnvelopesAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Envelopes/Index";
                var response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var envelope = JsonConvert.DeserializeObject<List<Envelope>>(json);
                    return envelope;
                }
            }
            throw new EmptyListException("The database is unavalailable.");
        }
        /// <summary>
        /// Returns all Envelopes for a user.
        /// </summary>
        /// <param name="accountid">The account ID to receive the Envelopes. </param>
        /// <returns></returns>
        public async Task<List<Envelope>> GetEnvelopeByAccountAsync(string accountid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Envelopes/AccountEnvelope?id=" + accountid;
                var response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var envelope = JsonConvert.DeserializeObject<List<Envelope>>(json);
                    return envelope;
                }
            }
            throw new NotFoundException("Given Account has no Envelope");
        }
        /// <summary>
        /// Returns an Envelope details.
        /// </summary>
        /// <param name="envelopeid">The envelope ID for which we want to get the details.</param>
        /// <returns></returns>
        public async Task<Envelope> GetEnvelopeDetailsAsync(int envelopeid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Envelopes/Details?id=" + envelopeid;
                var response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var envelope = JsonConvert.DeserializeObject<Envelope>(json);
                    return envelope;
                }
            }
            throw new NotFoundException("Envelope not found by ID");
        }
        /// <summary>
        /// Returns an Envelope details with its Transactions.
        /// </summary>
        /// <param name="envelopeid">The envelope ID for which we want to get the details.</param>
        /// <returns></returns>
        public async Task<Envelope> GetEnvelopeDetailsWithTransactionAsync(int envelopeid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Envelopes/DetailsWithTransaction?id=" + envelopeid;
                var response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var envelope = JsonConvert.DeserializeObject<Envelope>(json);
                    return envelope;
                }
            }
            throw new NotFoundException("Envelope not found by ID");
        }
        /// <summary>
        /// Create the given envelope.
        /// </summary>
        /// <param name="envelope">The envelope we want to create.</param>
        /// <returns></returns>
        public async Task<Envelope> CreateEnvelopeAsync(Envelope envelope)
        {
            if (envelope == null)
                return null;
            var package = await Task.Run(() => JsonConvert.SerializeObject(envelope));
            var myContent = new StringContent(package, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Envelopes/Create";
                var response = await client.PostAsync(route, myContent);
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<Envelope>(await response.Content.ReadAsStringAsync());
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new NotFoundException("Invalid input envelope");
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new NotFoundException("Foreign key missing");
                }
            }
            throw new ServiceConnectException("Service unavailable");

        }
        /// <summary>
        /// Returns the details of an envelope we wants to delete.
        /// </summary>
        /// <param name="envelopeid">Envelope ID that we want to delete.</param>
        /// <returns></returns>
        public async Task<Envelope> EditAsync(int envelopeid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Envelopes/Edit?id=" + envelopeid;
                var response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var envelope = JsonConvert.DeserializeObject<Envelope>(json);
                    return envelope;
                }
            }
            throw new NotFoundException("Envelope not found by ID");
        }
        /// <summary>
        /// Delete the envelope from the database.
        /// </summary>
        /// <param name="envelope">Envelope ID that we want to delete.</param>
        /// <returns></returns>
        public async Task<Envelope> EditEnvelopeAsync(Envelope envelope)
        {
            if (envelope == null)
                return null;
            var package = await Task.Run(() => JsonConvert.SerializeObject(envelope));
            var myContent = new StringContent(package, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Envelopes/EditEnvelope?id=" + envelope.Id;
                var response = await client.PostAsync(route, myContent);
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<Envelope>(await response.Content.ReadAsStringAsync());
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new NotFoundException("Invalid input envelope");

                    case HttpStatusCode.NotFound:
                        throw new NotFoundException("Edited envelope not found");

                    case HttpStatusCode.Conflict:
                        throw new NotFoundException("Envelope saving failed in DB");
                    default:
                        throw new Exception("Unhandled Statuscode");
                }
            }
            throw new ServiceConnectException("Service unavailable");
        }
        /// <summary>
        /// Returns the details of an envelope we wants to edit.
        /// </summary>
        /// <param name="envelopeid">Envelope ID that we want to edit.</param>
        /// <returns></returns>
        public async Task<Envelope> DeleteAsync(int envelopeid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Envelopes/Delete?id=" + envelopeid;
                var response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var envelope = JsonConvert.DeserializeObject<Envelope>(json);
                    return envelope;
                }
            }
            throw new NotFoundException("Envelope not found by ID");
        }
        /// <summary>
        /// Update the given envelope in the database.
        /// </summary>
        /// <param name="envelopeid">Envelope ID that we want to edit.</param>
        /// <returns></returns>
        public async Task<bool> DeleteEnvelopeConfirmedAsync(int envelopeid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Envelopes/DeleteConfirmed?id=" + envelopeid;
                var response = await client.DeleteAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else throw new NotFoundException("Transaction not found");
            }
        }



    }
}
