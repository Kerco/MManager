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

        public async Task LogOutAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Accounts/Logout";
                var response = await client.PostAsync(route, null);
            }
        }

        public async Task ClearCookiesAsnyc()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseAddr;
                string route = "Accounts/LoginClear";
                await client.GetAsync(route);
            }
        }

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


        public async Task ExternalLoginAsync(string provider, string returnUrl)
        {
            using (var client = new HttpClient())
            {
                if (provider == null)
                    return;
                var package = await Task.Run(() => JsonConvert.SerializeObject(provider));
                var myContent = new StringContent(package, Encoding.UTF8, "application/json");

                var package2 = await Task.Run(() => JsonConvert.SerializeObject(returnUrl));
                var myContent2 = new StringContent(package, Encoding.UTF8, "application/json");

                client.BaseAddress = baseAddr;
                string route = "Accounts/ExternalLogin";
            }
        }
    }
}
