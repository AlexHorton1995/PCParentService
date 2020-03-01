using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("PCParentTestProject")]
namespace PCParentServiceApp
{
    /// <summary>
    /// This class retrieves the username and password that authenticates against the Windows Live account
    /// </summary>
    class AuthenticationClass : IAuthenticationClass
    {
        public bool CreateCredentials()
        {
            string TestKey1 = "someemail@outlook.com";
            string TestKey2 = "somepassword";

            //Now, let's set three keys in the machine registry that will store our masked entries
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("PCParentKeys");

            var byteArr = Encoding.Unicode.GetBytes(TestKey1);

            key.SetValue("PCPUser", Convert.ToBase64String(Encoding.Unicode.GetBytes(TestKey1)));
            key.SetValue("PCPPass", Convert.ToBase64String(Encoding.Unicode.GetBytes(TestKey2)));
            key.Close();

            return true;
        }

        public List<string> RetrieveCredentials()
        {
            List<string> retStr = new List<string>();

            //logic to retrieve a registry key value
            const string rt = "HKEY_CURRENT_USER";
            const string sk = "TestKeys";  //your original key name will go here.
            const string kn = rt + "\\" + sk;

            //get the keys out of the registry.
            var keya = Microsoft.Win32.Registry.GetValue(kn, "PCPUser", "novalue");
            var keyb = Microsoft.Win32.Registry.GetValue(kn, "PCPPass", "novalue");




            return retStr;
        }



    }
}
