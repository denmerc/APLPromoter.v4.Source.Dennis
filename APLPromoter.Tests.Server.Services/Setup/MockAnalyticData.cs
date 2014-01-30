using APLPromoter;
using APLPromoter.Server.Data;
using APLPromoter.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLPromoter.Tests.Server.Services
{
    public class MockAnalyticData : IAnalyticData
    {
        private readonly List<Analytic.Identity> _SavedAnalytics = new List<Analytic.Identity>();
        private readonly List<Filter> _SavedFilters = new List<Filter>();
        private readonly List<Analytic.Type> _SavedTypes = new List<Analytic.Type>();

        private SqlService _SqlService;
        

        public MockAnalyticData() { }
        public MockAnalyticData(SqlService sql)
        {
            this._SqlService = sql;
        }

        public Session<Analytic.Identity> SaveIdentity(Session<Analytic.Identity> session)
        {
            if ((Convert.ToInt32(session.SqlKey)) == (int) Role.NonAdmin)
            {
                return new Session<Analytic.Identity>() { ServerMessage = "Security violation - user cannot add analytics" };
            }
            if (session.Data.Name.Length < 5 ||
                        session.Data.Name.Length > 50)
            {
                return new Session<Analytic.Identity>() { ServerMessage = "Invalid Name" };
            }
            if (session.Data.Description.Length < 5  ||
                        session.Data.Description.Length > 150 )
            {
                return new Session<Analytic.Identity>() { ServerMessage = "Invalid Description" };
            }

            _SavedAnalytics.Add(session.Data);
            return new Session<Analytic.Identity>() { ServerMessage = "Success-Saving Analytic.Identity" };
        }

        public Session<List<Analytic.Identity>> LoadList(Session<NullT> session)
        {
            return new Session<List<Analytic.Identity>>() { Data = _SavedAnalytics };
        }

        public Session<List<Filter>> LoadFilters(Session<Analytic.Identity> session)
        {
            return new Session<List<Filter>>() { Data = _SavedFilters };
        }

        public Session<List<Filter>> SaveFilters(Session<Analytic> session)
        {
            if (session.Data.Filters == null || session.Data.Filters.Count < 1) { return new Session<List<Filter>> { ServerMessage = "Error - No Filters" }; };
            _SavedFilters.AddRange(session.Data.Filters);
            return new Session<List<Filter>> { ServerMessage = "SavedFilters" };
        }

        public Session<List<Analytic.Type>> LoadTypes(Session<Analytic.Identity> session)
        {
            return new Session<List<Analytic.Type>> { Data = _SavedTypes };
        }

        public Session<List<Analytic.Type>> SaveTypes(Session<Analytic> session)
        {
            var typesWithInvalidNumGroups = (
                from type in session.Data.Types
                from mode in type.Modes
             where mode.Groups.Count <= 0 || mode.Groups.Count > 15
                         select mode.Groups).ToList();
            
            var groupsWithInvalidOutliers = (from t in session.Data.Types
                                             from m in t.Modes
                                             from g in m.Groups
                                             where g.MaxOutlier < g.MinOutlier
                                             select g).ToList(); 


            if (session.Data.Types == null || typesWithInvalidNumGroups.Count >= 1 || groupsWithInvalidOutliers.Count >= 1)
            {
                return new Session<List<Analytic.Type>> {ServerMessage = "Analytic.Types.Save - Invalid Groups"};
            };
            _SavedTypes.AddRange(session.Data.Types);
            return new Session<List<Analytic.Type>> { ServerMessage = "Analytic.Types.Save - Success" };
        }
    }

}
