using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APLPromoter.Client.Contracts;
using System.ServiceModel;
using APLPromoter.Client.Entity;
using System.ComponentModel.Composition;

namespace APLPromoter.Client
{
    [Export]
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

        public Session<List<Client.Entity.Analytic.Driver>> LoadDrivers(Session<Client.Entity.NullT> session)
        {
            return Channel.LoadDrivers(session);
        }

        public Session<List<Client.Entity.Analytic.Driver>> SaveDrivers(Session<Client.Entity.Analytic> session)
        {
            return Channel.SaveDrivers(session);
        }


        public Session<List<PriceList>> LoadPriceLists(Session<Analytic.Identity> session)
        {
            return Channel.LoadPriceLists(session);
        }

        public Session<List<PriceList>> SavePriceLists(Session<Analytic> session)
        {
            return Channel.SavePriceLists(session);
        }
    }
}
