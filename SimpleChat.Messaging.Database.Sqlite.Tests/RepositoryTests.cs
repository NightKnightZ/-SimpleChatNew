using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Moq;
using SimpleChat.Messaging.Database.Sqlite;
using SimpleChat.Messaging.Database.Sqlite.Interfaces;
using SimpleChat.Messaging.Entities;

namespace SimpleChat.Messaging.Database.Sqlite.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        static string dbPath = Directory.GetCurrentDirectory();
        const string dbName = "SCTestDB.db3";
        string dbFullPath = $"{dbPath}\\{dbName}";
        IDatabaseSettings dbSettings;
        User user;
        Message message;
        MessagingRepository<User> userRepository;
        MessagingRepository<Message> messageRepository;

        [TestInitialize]
        public void Init()
        {
            if (File.Exists(dbFullPath))
                File.Delete(dbFullPath);
            dbSettings = Mock.Of<IDatabaseSettings>();
            dbSettings.ConnectionString = dbFullPath;
        }

        [TestMethod]
        public void AddUpdateDeleteUser()
        {
            userRepository = new MessagingRepository<User>(dbSettings);
            user = new User { Id = 1, Login = "NK" };
            userRepository.Add(user);
            userRepository.Save();

            var userFromDb = userRepository.GetItem(1);

            Assert.IsNotNull(userFromDb);
            Assert.AreEqual(userFromDb.Id, 1);
            Assert.AreEqual(userFromDb.Login, "NK");

            user.Login = "neNK";
            userRepository.Update(user);
            userRepository.Save();

            userFromDb = userRepository.GetItem(1);

            Assert.IsNotNull(userFromDb);
            Assert.AreEqual(userFromDb.Id, 1);
            Assert.AreEqual(userFromDb.Login, "neNK");

            userRepository.DeleteItem(1);
            userRepository.Save();

            userFromDb = userRepository.GetItem(1);

            Assert.IsNull(userFromDb);

            userRepository.Dispose();
        }

        [TestMethod]
        public void AddUpdateDeleteMessage()
        {
            messageRepository = new MessagingRepository<Message>(dbSettings);
            message = new Message { Id = 1, UserId=1, Text = "NK" };
            messageRepository.Add(message);
            messageRepository.Save();

            var messageFromDb = messageRepository.GetItem(1);

            Assert.IsNotNull(messageFromDb);
            Assert.AreEqual(messageFromDb.Id, 1);
            Assert.AreEqual(messageFromDb.UserId, 1);
            Assert.AreEqual(messageFromDb.Text, "NK");

            message.Text = "neNK";
            messageRepository.Update(message);
            messageRepository.Save();

            messageFromDb = messageRepository.GetItem(1);

            Assert.IsNotNull(messageFromDb);
            Assert.AreEqual(messageFromDb.Id, 1);
            Assert.AreEqual(messageFromDb.UserId, 1);
            Assert.AreEqual(messageFromDb.Text, "neNK");

            messageRepository.DeleteItem(1);
            messageRepository.Save();

            messageFromDb = messageRepository.GetItem(1);

            Assert.IsNull(messageFromDb);

            messageRepository.Dispose();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(dbFullPath))
                File.Delete(dbFullPath);
        }
    }
}
