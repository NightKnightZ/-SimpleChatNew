using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Moq;
using SimpleChat.Messaging.Database.Sqlite.Interfaces;
using SimpleChat.Messaging.Entities;

namespace SimpleChat.Messaging.Database.Sqlite.Tests
{
    [TestClass]
    public class ContextTests
    {
        static string dbPath = Directory.GetCurrentDirectory();
        const string dbName = "SCTestDB.db3";
        string dbFullPath = $"{dbPath}\\{dbName}";
        IDatabaseSettings dbSettings;
        MessagingContext context;
        User user;
        Message message;

        [TestInitialize]
        public void Init()
        {
            if (File.Exists(dbFullPath))
                File.Delete(dbFullPath);
            dbSettings = Mock.Of<IDatabaseSettings>();
            dbSettings.ConnectionString = dbFullPath;
            context = new MessagingContext(dbSettings);
            context.Database.EnsureCreated();
        }

        [TestMethod]
        public void AddUpdateDeleteUser()
        {
            user = new User { Id = 1, Login = "NK" };
            context.Users.Add(user);
            context.SaveChanges();
            var userFromDb = context.Users.Find(1);
            Assert.IsNotNull(userFromDb);
            Assert.AreEqual(userFromDb.Id, 1);
            Assert.AreEqual(userFromDb.Login, "NK");

            user.Login = "neNK";
            context.Users.Update(user);
            context.SaveChanges();
            userFromDb = context.Users.Find(1);
            Assert.IsNotNull(userFromDb);
            Assert.AreEqual(userFromDb.Id, 1);
            Assert.AreEqual(userFromDb.Login, "neNK");

            context.Users.Remove(user);
            context.SaveChanges();
            userFromDb = context.Users.Find(1);
            Assert.IsNull(userFromDb);
        }

        [TestMethod]
        public void AddUpdateDeleteMessage()
        {
            message = new Message { Id = 1, UserId=1, Text = "msg" };
            context.Messages.Add(message);
            context.SaveChanges();
            var messageFromDb = context.Messages.Find(1);
            Assert.IsNotNull(messageFromDb);
            Assert.AreEqual(messageFromDb.Id, 1);
            Assert.AreEqual(messageFromDb.UserId, 1);
            Assert.AreEqual(messageFromDb.Text, "msg");

            message.Text = "neNK";
            context.Messages.Update(message);
            context.SaveChanges();
            messageFromDb = context.Messages.Find(1);
            Assert.IsNotNull(messageFromDb);
            Assert.AreEqual(messageFromDb.Id, 1);
            Assert.AreEqual(messageFromDb.UserId, 1);
            Assert.AreEqual(messageFromDb.Text, "neNK");

            context.Messages.Remove(message);
            context.SaveChanges();
            messageFromDb = context.Messages.Find(1);
            Assert.IsNull(messageFromDb);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
            if (File.Exists(dbFullPath))
                File.Delete(dbFullPath);
        }
    }
}
