using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using APLPromoter.Server.Entity;

namespace APLPromoter.Server.Services.Contracts
{
    [ServiceContract]
    public interface IAnalyticService
    {
        [OperationContract]
        Session<List<Server.Entity.Analytic.Identity>> LoadList(Session<Server.Entity.NullT> session);
        [OperationContract]
        Session<Server.Entity.Analytic.Identity> SaveIdentity(Session<Server.Entity.Analytic.Identity> session);
        [OperationContract]
        Session<List<Server.Entity.Filter>> LoadFilters(Session<Server.Entity.Analytic.Identity> session);
        [OperationContract]
        Session<List<Server.Entity.Filter>> SaveFilters(Session<Server.Entity.Analytic> session);
        [OperationContract]
        Session<List<Server.Entity.Analytic.Type>> LoadTypes(Session<Server.Entity.Analytic.Identity> session);
        [OperationContract]
        Session<List<Server.Entity.Analytic.Type>> SaveTypes(Session<Server.Entity.Analytic> session);

    }
}
