

using APLPromoter.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLPromoter.Server.Data
{

    public interface IAnalyticData
    {   
        Session<List<Server.Entity.Analytic.Identity>> LoadList(Session<Server.Entity.NullT> session);
        Session<Server.Entity.Analytic.Identity> SaveIdentity(Session<Server.Entity.Analytic.Identity> session);
        Session<List<Server.Entity.Filter>> LoadFilters(Session<Server.Entity.Analytic.Identity> session);
        Session<List<Server.Entity.Filter>> SaveFilters(Session<Server.Entity.Analytic> session);
        Session<List<Server.Entity.Analytic.Type>> LoadTypes(Session<Server.Entity.NullT> session);
        Session<List<Server.Entity.Analytic.Type>> SaveTypes(Session<Server.Entity.Analytic> session);
    }

}
