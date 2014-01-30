using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLPromoter.Client.Contracts
{
    public interface IServiceFactory
    {
        T CreateClient<T>();
    }
}
