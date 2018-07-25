using System;
using Microsoft.EntityFrameworkCore;
using SimpleChat.Messaging.Entities;
using SimpleChat.Messaging.Database.Sqlite.Interfaces;
namespace SimpleChat.Messaging.Database.Sqlite
{
    public class MessagingContext : DbContext
    {
        IDatabaseSettings dbSettings;
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        public MessagingContext(IDatabaseSettings dbSettings)
        {
            CheckSettings(dbSettings);
            
        }

        private void CheckSettings(IDatabaseSettings dbSettings)
        {
            
            if (dbSettings == null)
                throw new ArgumentNullException(nameof(dbSettings));

            if (string.IsNullOrWhiteSpace(dbSettings.ConnectionString))
                throw new ArgumentException("Database connection string cannot be null or white space");
            this.dbSettings = dbSettings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"FileName={dbSettings.ConnectionString}");
        }
    }
}
