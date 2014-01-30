using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLPromoter.Tests.Client.Setup.Data
{
    public interface IDataStore
    {
        IEnumerable<T> GetAll<T>();
        void Add<T>(T instance);
        void AddRange<T>(IEnumerable<T> instances);
        void ClearData<T>();
    }
}
