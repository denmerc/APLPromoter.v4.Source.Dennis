using APLPromoter.UI.Wpf.ViewModel;
using ReactiveUI;
using System.Windows;

namespace APLPromoter.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for AnalyticEditView.xaml
    /// </summary>
    public partial class AnalyticEditView : IViewFor<AnalyticViewModel>
    {
        public AnalyticEditView()
        {
            InitializeComponent();
            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);
            this.Bind(ViewModel, x => x.Name, x => x.xModelName.Text);
            this.Bind(ViewModel, x => x.HashCode, x => x.id.Text);
        }

        public AnalyticViewModel ViewModel
        {
            get
            {
                return (AnalyticViewModel)GetValue(ViewModelProperty);
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
            set { ViewModel = (AnalyticViewModel)value; }
        }
        public static readonly DependencyProperty ViewModelProperty =
DependencyProperty.Register("ViewModel", typeof(AnalyticViewModel), typeof(AnalyticEditView), new PropertyMetadata(null));
    }
}
