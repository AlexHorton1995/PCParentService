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

            //logic to retrieve a registry key value
            string rt = "HKEY_CURRENT_USER";
            string sk = "PCParentKeys";  //your original key name will go here.
            string kn = string.Format(@"{0}\{1}", rt, sk);

            //get the keys out of the registry.
            var userNameKey = Microsoft.Win32.Registry.GetValue(kn, "PCPUser", "novalue");
            var passWordKey = Microsoft.Win32.Registry.GetValue(kn, "PCPPass", "novalue");

            var userNameRes = Encoding.Unicode.GetString(Convert.FromBase64String(userNameKey.ToString()));
            var passWordRes = Encoding.Unicode.GetString(Convert.FromBase64String(passWordKey.ToString()));

            retList.Add(new KeyValuePair<string, string>("User", userNameRes));
            retList.Add(new KeyValuePair<string, string>("Password", passWordRes));

            return retList;
        }



    }
}
