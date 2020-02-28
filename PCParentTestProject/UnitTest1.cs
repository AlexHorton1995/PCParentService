using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCParentServiceApp;

namespace PCParentTestProject
{
    [TestClass]
    public class UnitTest1
    {
        IMailClientNotify mockNotify;

        [TestInitialize]
        public void TestInitialize()
        {
            mockNotify = new PCParentServiceApp.MailClientNotify(); 
        }

        [TestMethod]
        public void TestMethod1()
        {
            mockNotify.SendEmailNotification(1);
          

            

        }
    }
}
