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
    public class TransactionManager
    {
        private readonly Uri baseAddr = new Uri("http://localhost:55388/");

        public async Task<List<Transaction>> ListTransactionAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Transactions/Index";
                var response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content;
                    var transaction = JsonConvert.DeserializeObject<List<Transaction>>(await content.ReadAsStringAsync());
                    return transaction;
                }
            }
            throw new EmptyListException("The database is unavalailable.");
        }

        public async Task<Transaction> GetTransactionDetailsAsync(int transactionid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Transactions/Details?id=" + transactionid;
                var response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var transaction = JsonConvert.DeserializeObject<Transaction>(content);
                    return transaction;
                }
            }
            throw new NotFoundException("Transaction not found by ID");
        }


        public async Task<List<Transaction>> GetTransactionByEnvelopeIdAsync(int envelopeid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Transactions/EnvelopeTransaction?id=" + envelopeid;
                var response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var transaction = JsonConvert.DeserializeObject<List<Transaction>>(content);
                    return transaction;
                }
            }
            throw new NotFoundException("Given Envelope has no Transaction");
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new NotFoundException("Given transaction is empty");
            }

            var json = await Task.Run(() => JsonConvert.SerializeObject(transaction));
            var myContent = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Transactions/Create";
                var response = await client.PostAsync(route, myContent);
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<Transaction>(await response.Content.ReadAsStringAsync());
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new NotFoundException("Invalid input transaction");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new NotFoundException("Creating failed in DB");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new NotFoundException("Foreign key missing");
                }
            }
            throw new ServiceConnectException("Service unavailable");
        }

        public async Task<Transaction> DeleteAsync(int transactionid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Transactions/Delete?id=" + transactionid;
                var response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var transaction = JsonConvert.DeserializeObject<Transaction>(content);
                    return transaction;
                }
            }
            throw new NotFoundException("Transaction not found by Id");
        }


        public async Task<bool> DeleteTransactionConfirmedAsync(int transactionid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Transactions/DeleteConfirmed?id=" + transactionid;
                var response = await client.DeleteAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else throw new NotFoundException("Transaction not found");
            }

        }

        public async Task<Transaction> EditAsync(int transactionid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Transactions/Edit?id=" + transactionid;
                var response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var transaction = JsonConvert.DeserializeObject<Transaction>(content);
                    return transaction;
                }
            }
            throw new NotFoundException("Transaction not found by Id");
        }


        public async Task<Transaction> EditTransactionAsync(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new NotFoundException("Given transaction is empty");
            }

            var json = await Task.Run(() => JsonConvert.SerializeObject(transaction));
            var myContent = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Transactions/EditTransaction?id=" + transaction.Id;
                var response = await client.PostAsync(route, myContent);
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<Transaction>(await response.Content.ReadAsStringAsync());
                }
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new NotFoundException("Invalid input transaction");

                    case HttpStatusCode.NotFound:
                        throw new NotFoundException("Edited transaction not found");

                    case HttpStatusCode.Conflict:
                        throw new NotFoundException("Transaction saving failed in DB");
                    default:
                        throw new Exception("Unhandled Statuscode");
                }
            }
            throw new ServiceConnectException("Service unavailable");
        }
    }
}
