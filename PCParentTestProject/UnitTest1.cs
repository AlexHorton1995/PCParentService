using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCParentServiceApp;

namespace PCParentTestProject
{
    [TestClass]
    public class UnitTest1
    {
        IMailClientNotify mockNotify;
        IAuthenticationClass mockAuthenticate;
        ILoggerClass mockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            mockNotify = new MailClientNotify();
            mockAuthenticate = new AuthenticationClass();
            mockLogger = new LoggerClass();
        }

        [TestMethod]
        public void TestEventViewerLogin()
        {
            mockLogger.WriteLoginToEventViewer();
        }

        [TestMethod]
        public void RetrieveCredentials()
        {
            var credentialKeys = mockAuthenticate.RetrieveCredentials();

            //test number of keys returned
            Assert.AreEqual(2, credentialKeys.Count);

            //Test KVPairs
            foreach (var kvPair in credentialKeys)
            {
                if (kvPair.Key == "User")
                    Assert.AreEqual(@"someemail@outlook.com", kvPair.Value);

                if (kvPair.Key == "Password")
                    Assert.AreEqual(@"somepassword", kvPair.Value);
            }
        }

        [TestMethod]
        public void TestBrowserSniffer()
        {
            var browserWindows = BrowserSniffer.PrintBrowserTabName();

            List<string> ListOfWords = new List<string>()
            {
                "youtube",
                "pandora",
                "wwe",
                "roman",
                "reigns",
                "sasha",
                "banks"
            };

            foreach (var word in ListOfWords)
            {
                foreach (var bw in browserWindows)
                {
                    Assert.IsFalse(bw.ToLower().Contains(word));
                }
            }

        }



        [TestMethod]
        public void TestSendEMailNotification()
        {
            Assert.IsTrue(mockNotify.SendEmailNotification(1));
        }


        [TestMethod]
        public void TestEventViewerLogout()
        {
            mockLogger.WriteLogoffToEventViewer();
        }
    }
}
