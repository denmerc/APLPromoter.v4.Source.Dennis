using APLPromoter.Client.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace APLPromoter.Client.Contracts
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        Session<Client.Entity.NullT> Initialize(Session<Client.Entity.NullT> session);
        [OperationContract]
        Task<Session<Client.Entity.NullT>> InitializeAsync(Session<Client.Entity.NullT> session);
        [OperationContract]
        Session<Client.Entity.NullT> Authenticate(Session<Client.Entity.NullT> session);
        [OperationContract]
        Session<Client.Entity.NullT> LoadExplorerPlanning(Session<Client.Entity.NullT> session);
        [OperationContract]
        Session<Client.Entity.NullT> LoadExplorerTracking(Session<Client.Entity.NullT> session);
        [OperationContract]
        Session<Client.Entity.NullT> LoadExplorerReporting(Session<Client.Entity.NullT> session);
        [OperationContract]
        Session<List<Client.Entity.User.Identity>> LoadList(Session<Client.Entity.NullT> session);
        [OperationContract]
        Session<Client.Entity.User.Identity> LoadIdentity(Session<Client.Entity.User.Identity> session);
        [OperationContract]
        Session<Client.Entity.User.Identity> SaveIdentity(Session<Client.Entity.User.Identity> session);
        [OperationContract]
        Session<Client.Entity.NullT> SavePassword(Session<Client.Entity.NullT> session);
    }
}
