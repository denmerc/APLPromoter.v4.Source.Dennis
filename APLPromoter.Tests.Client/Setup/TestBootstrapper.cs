using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition;
using System.Reflection;
//using APLPromoter.Tests.Client.Setup.Data;
using APLPromoter.Client.Contracts;
using APLPromoter.Client;

namespace APLPromoter.Tests.Client.Setup
{
    class TestBootstrapper : Bootstrapper<IShellViewModel>
    {

        private CompositionContainer _container;

        public TestBootstrapper()
        {
            Start();
        }

        protected override void Configure()
        {
            _container = new CompositionContainer(
                new AggregateCatalog(
                    new AssemblyCatalog(typeof(IShellViewModel).Assembly),
                    AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>().FirstOrDefault()
                    )
            );

            var batch = new CompositionBatch();

            //TODO: plugin datastore for service
            //var testDataStore = new InMemoryDataStore();
            //batch.AddExportedValue<IDataStore>(testDataStore);
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            
            batch.AddExportedValue<IAnalyticService>(new AnalyticClient());
            batch.AddExportedValue(_container);

            _container.Compose(batch);

        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;

            var exports = _container.GetExportedValues<object>(contract);
            return exports.FirstOrDefault();
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            var ret = Enumerable.Empty<object>();

            string contract = AttributedModelServices.GetContractName(serviceType);
            return _container.GetExportedValues<object>(contract);
        }

        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }

    }
}
