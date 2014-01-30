using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APLPromoter.Client.Contracts;
using System.ServiceModel;
using APLPromoter.Client.Entity;

namespace APLPromoter.Client.Proxies
{
    public class AnalyticClient : ClientBase<IAnalyticService>, IAnalyticService
    {
        public Session<List<Client.Entity.Analytic.Identity>> LoadList(Session<Client.Entity.NullT> session)
        {
            return Channel.LoadList(session);
        }

        public Session<Client.Entity.Analytic.Identity> SaveIdentity(Session<Analytic.Identity> session){
            return Channel.SaveIdentity(session);
        }

        public Session<List<Client.Entity.Filter>> LoadFilters(Session<Client.Entity.Analytic.Identity> session)
        {
            return Channel.LoadFilters(session);
        }

        public Session<List<Client.Entity.Filter>> SaveFilters(Session<Client.Entity.Analytic> session)
        {
            return Channel.SaveFilters(session);
        }

        public Session<List<Client.Entity.Analytic.Type>> LoadTypes(Session<Client.Entity.NullT> session)
        {
            return Channel.LoadTypes(session);
        }

        public Session<List<Client.Entity.Analytic.Type>> SaveTypes(Session<Client.Entity.Analytic> session)
        {
            return Channel.SaveTypes(session);
        }
    }
}
