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
        IBrowserSniffer mockSniffer;

        [TestInitialize]
        public void TestInitialize()
        {
            mockNotify = new MailClientNotify();
            mockAuthenticate = new AuthenticationClass();
            mockLogger = new LoggerClass();
            mockSniffer = new BrowserSniffer();
        }

        [TestMethod]
        public void TestEventViewerLogin()
        {
            mockLogger.WriteLoginToEventViewer();
        }

        [TestMethod]
        public void CreateCredentials()
        {
            string TestKey1 = "someemail@outlook.com";
            string TestKey2 = "somepassword";

            //Now, let's set three keys in the machine registry that will store our masked entries
            Microsoft.Win32.RegistryKey userKey = Microsoft.Win32.Registry.Users.OpenSubKey("Software", true);
            userKey.CreateSubKey("PCParentKeys");
            var byteArr = Encoding.Unicode.GetBytes(TestKey1);

            userKey.SetValue("PCPUser", Convert.ToBase64String(Encoding.Unicode.GetBytes(TestKey1)));
            userKey.SetValue("PCPPass", Convert.ToBase64String(Encoding.Unicode.GetBytes(TestKey2)));
            userKey.Close();

            Assert.IsTrue(true);
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
            var browserWindows = mockSniffer.PrintBrowserTabName();

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
