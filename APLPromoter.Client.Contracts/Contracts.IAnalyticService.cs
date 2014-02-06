using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using APLPromoter.Client.Entity;

namespace APLPromoter.Client.Contracts
{
    [ServiceContract]
    public interface IAnalyticService
    {
        [OperationContract]
        Session<List<Client.Entity.Analytic.Identity>> LoadList(Session<Client.Entity.NullT> session);
        [OperationContract]
        Session<Client.Entity.Analytic.Identity> SaveIdentity(Session<Client.Entity.Analytic.Identity> session);
        [OperationContract]
        Session<List<Client.Entity.Filter>> LoadFilters(Session<Client.Entity.Analytic.Identity> session);
        [OperationContract]
        Session<List<Client.Entity.Filter>> SaveFilters(Session<Client.Entity.Analytic> session);
        [OperationContract]
        Session<List<Client.Entity.Analytic.Type>> LoadTypes(Session<Client.Entity.NullT> session);
        [OperationContract]
        Session<List<Client.Entity.Analytic.Type>> SaveTypes(Session<Client.Entity.Analytic> session);
    }
}
