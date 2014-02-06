using APLPromoter.UI.Wpf.ViewModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace APLPromoter.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for PriceRoutineEditView.xaml
    /// </summary>
    public partial class PriceRoutineEditView : IViewFor<PriceRoutineViewModel>
    {
        public PriceRoutineEditView()
        {
            InitializeComponent();
            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);
            this.Bind(ViewModel, x => x.Name, x => x.xModelName.Text);
        }

        public PriceRoutineViewModel ViewModel
        {
            get
            {
                return (PriceRoutineViewModel)GetValue(ViewModelProperty);
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
            set { ViewModel = (PriceRoutineViewModel)value; }
        }

        public static readonly DependencyProperty ViewModelProperty =
                DependencyProperty.Register("ViewModel", typeof(PriceRoutineViewModel), typeof(PriceRoutineEditView), new PropertyMetadata(null));
    }
}
