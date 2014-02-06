
using Promoter.Domain;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace APLPromoter.UI.Wpf.ViewModel
{
    public class ExplorerViewModel :  ReactiveObject
    {
        public ExplorerViewModel(string xmlTree)
        {

        }
        public ExplorerViewModel(ObservableCollection<Promoter.Domain.Analytic> analytics, 
                                                ObservableCollection<PriceRoutine> priceRoutines)
        {
            Analytics = analytics;
            PriceRoutines = priceRoutines;

            RootNodes = new ObservableCollection<INode>();
            RootNodes.Add(new ExplorerARootNode{Name="Analytics", Analytics=analytics});
            RootNodes.Add(new ExplorerPRootNode { Name = "Price Routines", PriceRoutines=priceRoutines });

            //NavigatedViewModels.Add(new HomeViewModel());
            //SelectedViewModel = NavigatedViewModels.FirstOrDefault();
        }

        private ReactiveObject _selectedViewModel;
        public ReactiveObject SelectedViewModel
        { 
            get
            {
                return _selectedViewModel;        
            }
            set
            {
                if(_selectedViewModel != value)
                {
                    _selectedViewModel = value;
                    this.RaiseAndSetIfChanged(ref _selectedViewModel, value);
                }
            }
        }
        public ObservableCollection<INode> RootNodes { get; set; }
        public ObservableCollection<Promoter.Domain.Analytic> Analytics { get; set; }
        public ObservableCollection<PriceRoutine> PriceRoutines { get; set; }

        
    }

    public class ExplorerRootNode : INode
    {
        public string Name { get; set; }
        public ObservableCollection<INode> Analytics { get; set; }
        public ObservableCollection<PriceRoutine> PriceRoutines { get; set; }
    }

    public class ExplorerARootNode : INode
    {
        public string Name { get; set; }
        public ObservableCollection<Analytic> Analytics { get; set; }
    }

    public class ExplorerPRootNode: INode
    {
        public string Name { get; set; }
        public ObservableCollection<PriceRoutine> PriceRoutines { get; set; }
    }

    public interface INode
    {
        
    }
}
