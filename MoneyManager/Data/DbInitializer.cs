using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Data
{
    public class DbInitializer
    {

        public static void Initalize(ManagerContext context)
        {
            try
            {

                if (context.Accounts.Any())
                {
                    return;
                }

                var accounts = new Account[]
                {
                new Account
                    {   Email = "abc@cba.hu",
                        PasswordHash = "jelszo",
                        UserName = "pesta",
                        Gender = true,
                        BirthDay = DateTime.Now,
                        Envelopes = new List<Envelope>()
                            {
                                new Envelope
                                    {
                                        Name = "Elelmiszer",
                                        Value = 10000,
                                        Details = "leiras"
                                    },
                                new Envelope
                                    {
                                        Name = "Rezsi",
                                        Value = 10500,
                                        Details = "leiras2"
                                    },
                                new Envelope
                                {

                                    Name = "Megtakaritas",
                                    Value = 12000,
                                    Details = "leiras3"

                                },
                            }
                },

                };

                context.Accounts.AddRange(accounts);
                context.SaveChanges();

                var transactions = Enumerable.Range(1, 5).Select(x =>
                        new Transaction
                        {
                            Name = "Transaction" + x,
                            Details = "leiras",
                            Date = DateTime.Now,
                            Value = 200,
                            Type = x % 2 == 1 ? true : false,
                            EnvelopeId = 1
                        });
                context.Transactions.AddRange(transactions);
                context.SaveChanges();


            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            context.Database.EnsureCreated();
            //    var acc = Enumerable.Range(1, 5){ }
        }
    }
}
