using System.Collections.Generic;

namespace PCParentServiceApp
{
    interface IAuthenticationClass
    {
        bool CreateCredentials();
        List<string> RetrieveCredentials();
    }
}