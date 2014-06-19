using APLPromoter.Client;
using APLPromoter.Client.Contracts;
//using APLPromoter.UI.Wpf.Services;
using APLPromoter.UI.Wpf.ViewModel;
using APLPromoter.UI.Wpf.Views;
using Ninject;
using Ninject.Modules;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Registration;
using System.Reflection;
using System.IO;

namespace APLPromoter.UI.Wpf.ViewModel
{
    public class ViewModelLocator
    {

        public static CompositionContainer Container { get; set; }
        private static StandardKernel kernel = null;
        public ViewModelLocator()
        {
            var conventions = new RegistrationBuilder();
            conventions.ForType<MainViewModel>().Export<MainViewModel>().SetCreationPolicy(CreationPolicy.Shared);
            conventions.ForType<ViewRouter>().Export<ViewRouter>().SetCreationPolicy(CreationPolicy.Shared);
            conventions.ForType<IUserService>().Export<UserClient>().SetCreationPolicy(CreationPolicy.Shared);
            conventions.ForType<IAnalyticService>().Export<AnalyticClient>().SetCreationPolicy(CreationPolicy.Shared);

            var appCatalog = new AssemblyCatalog(typeof(App).Assembly, conventions);
            var proxyCatalog = new AssemblyCatalog(typeof(APLPromoter.Client.AnalyticClient).Assembly, conventions);

            
            //var serviceCatalog = new AssemblyCatalog(typeof(APLPromoter.Client.Contracts.IUserService).Assembly, conventions);
            //var dirCatalog = new DirectoryCatalog(@".");
            //var dirCatalog = new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
            //    "APLPromoter.Client.*.dll");


            //var aggregate = new AggregateCatalog(dirCatalog);
            var aggregate = new AggregateCatalog(appCatalog, proxyCatalog);
            //var aggregate = new AggregateCatalog(appCatalog, serviceCatalog, proxyCatalog);
            Container = new CompositionContainer(aggregate);
            
            
            //Container = new CompositionContainer(new AssemblyCatalog(typeof(App).Assembly, conventions));

            Container.ComposeExportedValue("Version", "1.0");

            //var batch = new CompositionBatch();
            //batch.AddExportedValue("Version", "1.0");

            Container.SatisfyImportsOnce(this, conventions);
            
            //Container.ComposeParts(this, conventions);
            

        }


        public string Version
        {
            get
            {
                return Container.GetExportedValue<string>("Version");
            }
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return Container.GetExportedValue<MainViewModel>();
                //return kernel.Get<MainViewModel>();
            }
        }

        public ViewRouter ViewRouter
        {
            get
            {
                return Container.GetExportedValue<ViewRouter>();
            }
        }

        public ViewModelLocator VMLocator
        {
            get
            {
                return Container.GetExportedValue<ViewModelLocator>();
            }
        }

        public IUserService UserService
        {
            get
            {
                return Container.GetExportedValue<UserClient>();
            }
        }

        public IAnalyticService AnalyticService
        {
            get
            {
                return Container.GetExportedValue<AnalyticClient>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }

}


namespace APLPromoter.UI.Wpf.Modules
{
    public class DesignTimeModules : NinjectModule
    {
        public override void Load()
        {
            Bind<MainViewModel>().To<MainViewModel>().InSingletonScope();
        }

    }

    public class RunTimeModules : NinjectModule
    {
        public override void Load()
        {
            Bind<ViewModelLocator>().ToSelf().InSingletonScope();
            Bind<APLPromoter.Client.Contracts.IAnalyticService>().To<AnalyticClient>()
                .InSingletonScope();
            
            Bind<IUserService>().To<UserClient>().InSingletonScope();

            Bind<ViewRouter>().ToSelf().InSingletonScope();

            //var userService = Kernel.Get<IAuthentication>();
                //.WithConstructorArgument("userService", userService);

            //Bind<IScreen>().To<ViewRouter>().WhenInjectedInto<LoginViewModel>();
            //Bind<IScreen>().To<ViewRouter>().WhenInjectedInto<MainViewModel>();

            Bind<MainViewModel>().ToSelf().InSingletonScope();

            Bind<AnalyticViewModel>().ToSelf().InTransientScope();
                //.WithConstructorArgument("service", aService);
        }
        
    }


} 


