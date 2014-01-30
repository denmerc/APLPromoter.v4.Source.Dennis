using APLPromoter.Client.Contracts;
using APLPromoter.Client.ViewModels;
using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace APLPromoter.Tests.Client {
    public class ShellViewModel : Screen {
        
        private IWindowManager _windowManager;
        private IAnalyticService _analyticProxy;
        private ExportFactory<AnalyticViewModel> _analyticViewModelFactory;

        [ImportingConstructor()]
        public ShellViewModel(
                IWindowManager windowManager,
                IAnalyticService analyticProxy,
                ExportFactory<AnalyticViewModel> analyticViewModelFactory)
        {
            this._windowManager = windowManager;
            this._analyticProxy = analyticProxy;
            this._analyticViewModelFactory = analyticViewModelFactory;
        }


    
    }

    [InheritedExport]
    public interface IShellViewModel
    {

    }
}