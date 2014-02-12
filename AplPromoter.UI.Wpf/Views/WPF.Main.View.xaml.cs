using APLPromoter.UI.Wpf.ViewModel;
using ReactiveUI;
using System;
using System.Windows;

namespace APLPromoter.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : IViewFor<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();
            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);


            this.Bind(ViewModel, x => x.SelectedViewModel, x => x.xDetailView.ViewModel);
            this.WhenAny(x => x.xTreeView.SelectedItem, x => { return x.GetValue(); })
                .Subscribe( x=> 
                {
                    if (x is Analytic)
                    {
                        var a = (Analytic) x;
                        var vm = new AnalyticViewModel(a);
                        this.ViewModel.SelectedViewModel = vm;
                        this.xDetailView.ViewModel = vm;
                    }
                    if (x is PriceRoutine)
                    {
                        var p = (PriceRoutine) x;
                        var vm = new PriceRoutineViewModel(p);
                        this.ViewModel.SelectedViewModel = vm;
                        this.xDetailView.ViewModel = vm;
                    }    
                        
                });
                //.Select(x => new AnalyticViewModel(x))
                //.BindTo(this, x => x.ViewModel.SelectedViewModel);
        }

        public MainViewModel ViewModel
        {
            get
            {
                return (MainViewModel)GetValue(ViewModelProperty);
            }
            set
            {
                SetValue(ViewModelProperty,
                    value);
            }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainViewModel)value; }
        }

        public static readonly DependencyProperty ViewModelProperty =
DependencyProperty.Register("ViewModel", typeof(MainViewModel), typeof(MainView), new PropertyMetadata(null));
    }
}
