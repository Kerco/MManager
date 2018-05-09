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
    class AccountManager
    {
        private readonly Uri baseAddr = new Uri("http://localhost:55388/");
        /// <summary>
        /// Returns an Account details.
        /// </summary>
        /// <param name="id">The Account ID for which we want to get the details.</param>
        /// <returns></returns>
        public async Task<Account> DetailsAsync(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Accounts/Details?id=" + id;
                var response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var acc = JsonConvert.DeserializeObject<Account>(json);
                    return acc;
                }
            }
            throw new NotFoundException("Account can not be found");
        }
        /// <summary>
        /// Register the given user.
        /// </summary>
        /// <param name="model">The model we want to Register.</param>
        /// <returns></returns>
        public async Task<Account> RegsiterAsync(RegisterModel model)
        {
            if (model == null)
                return null;

            var package = await Task.Run(() => JsonConvert.SerializeObject(model));
            var myContent = new StringContent(package, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Accounts/Register";
                var response = await client.PostAsync(route, myContent);
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<Account>(await response.Content.ReadAsStringAsync());
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new NotFoundException("Invalid input account");

            }
            throw new ServiceConnectException("Service unavailable");
        }
        /// <summary>
        /// Returns an Account details by E-mail address.
        /// </summary>
        /// <param name="email">The Account E-Mail address for which we want to get the details.</param>
        /// <returns></returns>
        public async Task<Account> GetAccountByEmail(string email)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Accounts/DetailsByEmail?email=" + email;
                var response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var acc = JsonConvert.DeserializeObject<Account>(json);
                    return acc;
                }
            }
            throw new NotFoundException("Account can not be found");
        }
        /// <summary>
        /// Returns an Account Details by UserName.
        /// </summary>
        /// <param name="username">The Account UserName for which we want to get the details.</param>
        /// <returns></returns>
        public async Task<Account> GetAccountByUserName(string username)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Accounts/DetailsByUserName?username=" + username;
                var response = await client.GetAsync(route);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var acc = JsonConvert.DeserializeObject<Account>(json);
                    return acc;
                }
            }
            throw new NotFoundException("Account can not be found");
        }
        /// <summary>
        /// Logout the current Account.
        /// </summary>
        /// <returns></returns>
        public async Task LogOutAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Accounts/Logout";
                var response = await client.PostAsync(route, null);
            }
        }
        /// <summary>
        /// Clear cookies to be sure everything is clear for a login.
        /// </summary>
        /// <returns></returns>
        public async Task ClearCookiesAsnyc()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Accounts/LoginClear";
                await client.GetAsync(route);
            }
        }
        /// <summary>
        /// Login the current Account.
        /// </summary>
        /// <param name="model">The model we wants to login.</param>
        /// <returns></returns>
        public async Task<Account> LogInAsync(LoginViewModel model)
        {
            using (var client = new HttpClient())
            {
                if (model == null)
                    return null;
                var package = await Task.Run(() => JsonConvert.SerializeObject(model));
                var myContent = new StringContent(package, Encoding.UTF8, "application/json");

                client.BaseAddress = baseAddr;
                string route = "Accounts/Login";
                var response = await client.PostAsync(route, myContent);
                if (response.IsSuccessStatusCode)
                {
                    return new Account();
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    return null;
            }
            throw new ServiceConnectException("Service unavailable");
        }
    }
}
