using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Data
{
    public class ManagerContext : IdentityDbContext<IdentityUser>
    {
        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Envelope> Envelopes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Account>().ToTable("Account");
            builder.Entity<Envelope>().ToTable("Envelope");
            builder.Entity<Transaction>().ToTable("Transaction");

            //      builder.Entity<Account>().HasKey(a => a.Id);
            //      builder.Entity<Account>().Property(a => a.Email).IsRequired();
            //       builder.Entity<Account>().Property(a => a.UserName).IsRequired();
            builder.Entity<Account>().Property(a => a.Gender).IsRequired();
            builder.Entity<Account>().Property(a => a.BirthDay).IsRequired();
            builder.Entity<Account>().HasMany(a => a.Envelopes).WithOne(e => e.Account);

            builder.Entity<Envelope>().HasKey(e => e.Id);
            builder.Entity<Envelope>().Property(e => e.Name).IsRequired();
            builder.Entity<Envelope>().Property(e => e.Value).IsRequired();
            builder.Entity<Envelope>().HasOne(e => e.Account).WithMany(a => a.Envelopes);
            builder.Entity<Envelope>().HasMany(e => e.Transactions).WithOne(t => t.Envelope);

            builder.Entity<Transaction>().HasKey(t => t.Id);
            builder.Entity<Transaction>().Property(t => t.Name).IsRequired();
            builder.Entity<Transaction>().Property(t => t.Date).IsRequired();
            builder.Entity<Transaction>().Property(t => t.Type).IsRequired();
            builder.Entity<Transaction>().Property(t => t.Value).IsRequired();
            builder.Entity<Transaction>().HasOne(t => t.Envelope).WithMany(e => e.Transactions);


        }

    }
}
