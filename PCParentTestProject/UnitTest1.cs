using System;
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

        [TestInitialize]
        public void TestInitialize()
        {
            mockNotify = new MailClientNotify();
            mockAuthenticate = new AuthenticationClass();
        }

        [TestMethod]
        public void CreateCredentials()
        {
            Assert.IsTrue(mockAuthenticate.CreateCredentials());
        }

        [TestMethod]
        public void RetrieveCredentials()
        {

        }


        [TestMethod]
        public void TestMethod1()
        {
            mockNotify.SendEmailNotification(1);

        }
    }
}
