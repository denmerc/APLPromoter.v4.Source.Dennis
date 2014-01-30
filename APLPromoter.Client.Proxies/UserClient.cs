using APLPromoter.Client.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace APLPromoter.Client.Proxies
{
    public class UserClient : ClientBase<IUserService>, IUserService
    {
        public APLPromoter.Client.Entity.User.Identity GetUser(Int32 id)
        {
            return Channel.GetUser(id);
        }
    }
}
