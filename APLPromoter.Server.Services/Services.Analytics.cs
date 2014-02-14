using APLPromoter.Server.Services.Contracts;
using APLPromoter.Server.Data;
using APLPromoter.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace APLPromoter.Server.Services
{
    public class AnalyticService : IAnalyticService
    {
        private IAnalyticData _analyticRepo;


        public AnalyticService() : this(new AnalyticData()) { }
        [ImportingConstructor]
        public AnalyticService(IAnalyticData analyticRepository)
        { 
            this._analyticRepo = analyticRepository;
        
        }

        public Session<Server.Entity.Analytic.Identity> SaveIdentity(Session<Server.Entity.Analytic.Identity> session)
        {
            return _analyticRepo.SaveIdentity(session);
        }

        public Session<List<Server.Entity.Analytic.Identity>> LoadList(Session<Server.Entity.NullT> session)
        {
            return _analyticRepo.LoadList(session);
        }


        public Session<List<Server.Entity.Filter>> LoadFilters(Session<Server.Entity.Analytic.Identity> session)
        {
            return _analyticRepo.LoadFilters(session);
        }



        public Session<List<Server.Entity.Filter>> SaveFilters(Session<Server.Entity.Analytic> session)
        {
            return _analyticRepo.SaveFilters(session);
        }

        public Session<List<Server.Entity.Analytic.Driver>> LoadDrivers(Session<Server.Entity.Analytic.Identity> session)
        {
            return _analyticRepo.LoadDrivers(session);
        }

        public Session<List<Server.Entity.Analytic.Driver>> SaveDrivers(Session<Server.Entity.Analytic> session)
        {
            return _analyticRepo.SaveDrivers(session);
        }


        public Session<List<PriceList>> LoadPriceLists(Session<Analytic.Identity> session)
        {
            return _analyticRepo.LoadPriceLists(session);
        }

        public Session<List<PriceList>> SavePriceLists(Session<Analytic> session)
        {
            return _analyticRepo.SavePriceLists(session);
        }
    }
}