using SimpleChat.Messaging.Interfaces;
using SimpleChat.Messaging.Database.Sqlite.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;

namespace SimpleChat.Messaging.Database.Sqlite
{
    public class MessagingRepository<T> : IRepository<T>, IDisposable
        where T : class
    {
        DbSet<T> dbset;
        MessagingContext context;
        IDatabaseSettings dbSettings;

        public MessagingRepository(IDatabaseSettings dbSettings)
        {
            CheckSettings(dbSettings);
            
            context = new MessagingContext(dbSettings);
            context.Database.EnsureCreated();
            dbset = context.Set<T>(); 
        }

        private void CheckSettings(IDatabaseSettings dbSettings)
        {
            if (dbSettings == null)
                throw new ArgumentNullException(nameof(dbSettings));

            if (string.IsNullOrWhiteSpace(dbSettings.ConnectionString))
                throw new ArgumentException("Database connection string cannot be null or white space");
            this.dbSettings = dbSettings;   
        }

        public int Count()
        {
            return dbset.CountAsync().Result;
        }

        public void Add(T item)
        {
            dbset.Add(item);
        }

        public T GetItem(int id)
        {
            return dbset.Find(id);
        }

        public void Update(T item)
        {
            dbset.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }

        public void DeleteItem(int id)
        {
            dbset.Remove(dbset.Find(id));
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
