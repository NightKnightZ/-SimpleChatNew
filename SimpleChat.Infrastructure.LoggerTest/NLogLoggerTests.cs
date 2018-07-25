using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using SimpleChat.Infrastructure.Logger;

namespace SimpleChat.Infrastructure.LoggerTest
{
    [TestClass]
    public class NLogLoggerTests
    {
        static string LogPath = "A:";
        static string LogFileName = "log.log";
        string LogFullPath = $"{LogPath}\\{LogFileName}";
        NLogLogger logger;

        [TestInitialize]
        public void Init()
        {
            if (File.Exists(LogFullPath))
                File.Delete(LogFullPath);
            logger = new NLogLogger(LogFullPath);
        }

        [TestMethod]
        public void TestError()
        {
            string msg = "error message";
            logger.Error(msg);
            StreamReader sr = new StreamReader(LogFullPath);
            Assert.IsTrue(sr.ReadToEnd().Contains(msg));
            sr.Close();
        }

        [TestMethod]
        public void TestErrorEx()
        {
            Exception e = new Exception();
            string msg = "error message";
            logger.Error(e, msg);
            StreamReader sr = new StreamReader(LogFullPath);
            Assert.IsTrue(sr.ReadToEnd().Contains(msg));
            sr.Close();
        }

        [TestMethod]
        public void TestWarn()
        {
            string msg = "warn message";
            logger.Warn(msg);
            StreamReader sr = new StreamReader(LogFullPath);
            Assert.IsTrue(sr.ReadToEnd().Contains(msg));
            sr.Close();
        }

        [TestMethod]
        public void TestInfo()
        {
            string msg = "info message";
            logger.Info(msg);
            StreamReader sr = new StreamReader(LogFullPath);
            Assert.IsTrue(sr.ReadToEnd().Contains(msg));
            sr.Close();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(LogFullPath))
                File.Delete(LogFullPath);
        }
    }
}
