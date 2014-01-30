using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLPromoter.Tests.Server.Services
{
    public class InMemoryData
    {

        private sealed class InMemoryRepository<T>
        {
            private readonly InMemoryDataMapper mapper;

        }

        public class InMemoryDataMapper
        {
            private readonly List<object> committed = new List<object> ();
            private readonly List<object> uncommittedInserts = new List<object>();
            private readonly List<object> uncommittedDeletes = new List<object>();

            public InMemoryDataMapper(){}

            public InMemoryDataMapper (params object[] committedEntities){
                foreach (var committedEntity in committedEntities)
                {
                    AddCommitted(committedEntity);
                }
            }

            public void AddCommitted(object committedEntity)
            {
                
            }

        }

    }
}
