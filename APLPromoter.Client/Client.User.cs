using APLPromoter.Client.Contracts;
using APLPromoter.Client.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace APLPromoter.Client
{
    public class UserClient : ClientBase<IUserService>, IUserService
    {
        public Session<Client.Entity.NullT> Initialize(Session<Client.Entity.NullT> session)
        {
            return Channel.Initialize(session);
        }

        public Task<Session<NullT>> InitializeAsync(Session<Client.Entity.NullT> session)
        {
            return Channel.InitializeAsync(session);
        }

        public Session<NullT> Authenticate(Session<NullT> session)
        {
            return Channel.Authenticate(session);
        }

        public Session<NullT> LoadExplorerPlanning(Session<NullT> session)
        {
            return Channel.LoadExplorerPlanning(session);
        }

        public Session<NullT> LoadExplorerTracking(Session<NullT> session)
        {
            return Channel.LoadExplorerTracking(session);
        }

        public Session<NullT> LoadExplorerReporting(Session<NullT> session)
        {
            return Channel.LoadExplorerReporting(session);
        }

        public Session<List<User.Identity>> LoadList(Session<NullT> session)
        {
            return Channel.LoadList(session);
        }

        public Session<User.Identity> LoadIdentity(Session<User.Identity> session)
        {
            return LoadIdentity(session);
        }

        public Session<User.Identity> SaveIdentity(Session<User.Identity> session)
        {
            return SaveIdentity(session);
        }

        public Session<NullT> SavePassword(Session<NullT> session)
        {
            return SavePassword(session);
        }
    }
}
