using APLPromoter.UI.Wpf.ViewModel;
using ReactiveUI;
using System.Windows;

namespace APLPromoter.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : IViewFor<HomeViewModel>
    {
        public HomeView()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty ViewModelProperty =
DependencyProperty.Register("ViewModel", typeof(HomeViewModel), typeof(HomeView), new PropertyMetadata(null));


        public HomeViewModel ViewModel
        {
            get
            {
                return (HomeViewModel)GetValue(ViewModelProperty);
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
            set { ViewModel = (HomeViewModel)value; }
        }
    }
}
