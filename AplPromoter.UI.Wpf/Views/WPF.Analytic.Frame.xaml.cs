using APLPromoter.UI.Wpf.ViewModel;
using ReactiveUI;
using System.Windows;

namespace APLPromoter.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for AnalyticFrame.xaml
    /// </summary>
    public partial class AnalyticFrame : IViewFor<AnalyticViewModel>
    {
        public AnalyticFrame()
        {
            InitializeComponent();
            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);
            this.Bind(ViewModel, x => x.Name, x => x.xName.Text);
            this.Bind(ViewModel, x => x.Id, x => x.xId.Text);
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
DependencyProperty.Register("ViewModel", typeof(AnalyticViewModel), typeof(AnalyticFrame), new PropertyMetadata(null));
    }
}
