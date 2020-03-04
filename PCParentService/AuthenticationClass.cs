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

        public List<KeyValuePair<string,string>> RetrieveCredentials()
        {
            var retList = new List<KeyValuePair<string, string>>();

            //Key for service is located under users/default keys
            var subkey1 = Microsoft.Win32.Registry.Users.OpenSubKey(@".DEFAULT")
                .OpenSubKey(@"Software")
                .OpenSubKey(@"PCParentKeys");

            //get the keys out of the registry.
            var userNameKey = subkey1.GetValue("PCPUser");
            var passWordKey = subkey1.GetValue("PCPPass");

            //convert the keys to regular strings
            var userNameRes = Encoding.Unicode.GetString(Convert.FromBase64String(userNameKey.ToString()));
            var passWordRes = Encoding.Unicode.GetString(Convert.FromBase64String(passWordKey.ToString()));

            //return the keys to use in SMTP service hit
            retList.Add(new KeyValuePair<string, string>("User", userNameRes));
            retList.Add(new KeyValuePair<string, string>("Password", passWordRes));

            return retList;
        }



    }
}
