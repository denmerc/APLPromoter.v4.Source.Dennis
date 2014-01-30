using APLPromoter.Core.Reactive;
using ReactiveUI;
using ReactiveUI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace APLPromoter.Client.ViewModels
{
    public class MainViewModel : ReactiveObject
    {

        public MainViewModel(IContentSource analyticModule,
                DashboardViewModel rightMargin,
                TreeViewModel leftMargin,
                MenuViewModel header)
        {
            ContentPanel = analyticModule;
            DashboardPanel = rightMargin;
            TreePanel =  leftMargin;
            MenuPanel = header;

            //var DisplayCommand = new ReactiveCommand(this.WhenAny(x => x.DashboardPanel.Count, x => !string.IsNullOrEmpty(x.Value)))
        
            

                
        }


        //Watch list in a analytic/price routine and update count in dashboard

        

        public IContentSource ContentPanel { get; private set; }
        public DashboardViewModel DashboardPanel { get; private set; }
        public TreeViewModel TreePanel { get; private set; }
        public MenuViewModel MenuPanel { get; private set; }

    }

    public class DashboardViewModel 
    {
        IEventAggregator _eventAggregator;
        public DashboardViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
        }
        public string Count { get; set; }
    }
    public class TreeViewModel { }
    public class MenuViewModel { }

    public interface IContentSource { }
}


