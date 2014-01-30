using APLPromoter.Client;
using APLPromoter.Client.Contracts;
//using APLPromoter.UI.Wpf.Services;
using APLPromoter.UI.Wpf.ViewModel;
using APLPromoter.UI.Wpf.Views;
using Ninject;
using Ninject.Modules;
using Promoter.Services;

namespace APLPromoter.UI.Wpf.ViewModel
{
    public class ViewModelLocator
    {
        private static StandardKernel kernel = null;
        public ViewModelLocator()
        {
            
            //TODO: determine if in design mode and load mock data
            //if (ViewModelBase.IsInDesignModeStatic) 
            //{
                // Create design time view services and models
                //kernel = new StandardKernel(new APLPromoter.UI.Wpf.Modules.DesignTimeModules());

            //}
            //else
            //{
                // Create run time view services and models
          
                kernel = new StandardKernel(new APLPromoter.UI.Wpf.Modules.RunTimeModules()); //right now only production module
            //}

        }

        public MainViewModel MainViewModel
        {
            get
            {
                return kernel.Get<MainViewModel>();
            }
        }

        public ViewRouter ViewRouter
        {
            get
            {
                return kernel.Get<ViewRouter>();
            }
        }

        public ViewModelLocator VMLocator
        {
            get
            {
                return kernel.Get<ViewModelLocator>();
            }
        }

        public UserService UserService
        {
            get
            {
                return kernel.Get<UserService>();
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
            Bind<IAuthentication>().To<UserService>().InSingletonScope();

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


