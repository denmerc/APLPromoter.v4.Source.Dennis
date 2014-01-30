using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promoter.Services
{
    public interface IAuthentication
    {
        AuthenticationResult Authenticate(string userName, string userPassword);
    }
    public class UserService : IAuthentication
    {
            public AuthenticationResult Authenticate(string userName, string password)
            {
                return userName == "dm" && password == "dm" ? AuthenticationResult.Authenticated
                    : AuthenticationResult.Failed;
            }
    }

        public enum AuthenticationResult
        {
            Authenticated,
            Failed
        }
    
}
