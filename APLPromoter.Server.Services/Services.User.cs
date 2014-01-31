using APLPromoter.Server.Services.Contracts;
using APLPromoter.Server.Data;
using APLPromoter.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace APLPromoter.Server.Services
{
    [ServiceBehavior(UseSynchronizationContext = false, 
                    InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Single)]
    //[CallbackBehavior(UseSynchronizationContext = false)]
    public class UserService : IUserService
    {
        private IUserData _userData;

        public UserService() : this(new UserData())
        { }
        public UserService(IUserData userData)
        {
            this._userData = userData;
        
        }


        public Session<NullT> Initialize(Session<NullT> session)
        {
            return _userData.Initialize(session);
        }

        public Session<NullT> Authenticate(Session<NullT> session)
        {
            return _userData.Authenticate(session);
        }

        public Session<NullT> LoadExplorerPlanning(Session<NullT> session)
        {
            return _userData.LoadExplorerPlanning(session);
        }

        public Session<NullT> LoadExplorerTracking(Session<NullT> session)
        {
            return _userData.LoadExplorerTracking(session);
        }

        public Session<NullT> LoadExplorerReporting(Session<NullT> session)
        {
            return _userData.LoadExplorerReporting(session);
        }

        public Session<List<User.Identity>> LoadList(Session<NullT> session)
        {
            return _userData.LoadList(session);
        }

        public Session<User.Identity> LoadIdentity(Session<User.Identity> session)
        {
            return _userData.LoadIdentity(session);
        }

        public Session<User.Identity> SaveIdentity(Session<User.Identity> session)
        {
            return _userData.SaveIdentity(session);
        }

        public Session<NullT> SavePassword(Session<NullT> session)
        {
            return _userData.SavePassword(session);
        }
    }
}
