using Promoter.Domain;
using APLPromoter.UI.Wpf.Views;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace APLPromoter.UI.Wpf.ViewModel
{

    public class MainViewModel : ReactiveObject, IRoutableViewModel
    {

        public MainViewModel(IScreen screen)
        {

                HostScreen = (ViewRouter)screen;
                Router = HostScreen.Router;
                Locator = App.Current.Resources["Locator"] as ViewModelLocator;

                ExplorerViewModel = new ExplorerViewModel(LoadAnalytics(), LoadPriceRoutines());

            //NavigatedViewModels.Add(new HomeViewModel());
            //SelectedViewModel = NavigatedViewModels.FirstOrDefault();
        }

        public  ViewModelLocator Locator { get; set; }
        public ExplorerViewModel ExplorerViewModel { get; set; }
        //public IViewModel DetailsViewModel { get; set; }
        
        public IScreen HostScreen { get; set; }
        public IRoutingState Router { get; set; }
        public string UrlPathSegment {get { return "main";}}

        private List<ReactiveObject> _navigatedViewModels;
        public List<ReactiveObject> NavigatedViewModels
        {
            get
            {
                if (_navigatedViewModels == null)
                    _navigatedViewModels = new List<ReactiveObject>();
                return _navigatedViewModels;
            }
        }
        private ReactiveObject _selectedViewModel = new HomeViewModel();
        public ReactiveObject SelectedViewModel
        {
            get
            {
                return _selectedViewModel;
            }
            set
            {
                if (_selectedViewModel != value)
                {
                    _selectedViewModel = value;
                    this.RaiseAndSetIfChanged(ref _selectedViewModel, value);
                }
            }
        }
        public ReactiveCommand Navigate { get; set; }
        public ReactiveCommand Refresh { get; set; }
        public ObservableCollection<Promoter.Domain.Analytic> Analytics { get; set; }
        public ObservableCollection<PriceRoutine> PriceRoutines { get; set; }

        public void ChangeViewModel(ReactiveObject viewModel)
        {
            if (!NavigatedViewModels.Contains(viewModel))
                NavigatedViewModels.Add(viewModel);
            SelectedViewModel = NavigatedViewModels.FirstOrDefault(vm => vm == viewModel);

        }
        public ObservableCollection<Promoter.Domain.Analytic> LoadAnalytics()
        {
            return new ObservableCollection<Promoter.Domain.Analytic>()
            {
                new Promoter.Domain.Analytic {Id=1, Name="Analytic Name 1", Description = "Description1"},
                new Promoter.Domain.Analytic {Id=2, Name="Analytic Name 2", Description = "Description2"},
                new Promoter.Domain.Analytic {Id=3, Name="Analytic Name 3", Description = "Description3"},
                new Promoter.Domain.Analytic {Id=4, Name="Analytic Name 4", Description = "Description4"},
                new Promoter.Domain.Analytic {Id=5, Name="Analytic Name 5", Description = "Description5"},
                new Promoter.Domain.Analytic {Id=6, Name="Analytic Name 6", Description = "Description6"},
                new Promoter.Domain.Analytic {Id=7, Name="Analytic Name 7", Description = "Desctiption7"}
            }; 
        }

        public ObservableCollection<Promoter.Domain.PriceRoutine> LoadPriceRoutines()
        {
            return new ObservableCollection<Promoter.Domain.PriceRoutine>()
            {
                new Promoter.Domain.PriceRoutine {Id=1, Name="Price Routine Name1", Description = "Description1"},
                new Promoter.Domain.PriceRoutine {Id=2, Name="Price Routine Name2", Description = "Description2"},
                new Promoter.Domain.PriceRoutine {Id=3, Name="Price Routine Name3", Description = "Description3"},
                new Promoter.Domain.PriceRoutine {Id=4, Name="Price Routine Name4", Description = "Description4"},
                new Promoter.Domain.PriceRoutine {Id=5, Name="Price Routine Name5", Description = "Description5"},
                new Promoter.Domain.PriceRoutine {Id=6, Name="Price Routine Name6", Description = "Description6"},
                new Promoter.Domain.PriceRoutine {Id=7, Name="Price Routine Name7", Description = "Desctiption7"}
            };
        }
    }
    public class TrackingTreeSource
    {
        public ObservableCollection<Promoter.Domain.Analytic> Analytics { get; set; }
    }
}
