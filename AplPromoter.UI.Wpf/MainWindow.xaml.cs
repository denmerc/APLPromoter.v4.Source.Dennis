using APLPromoter.UI.Wpf.ViewModel;
using System.Windows;

namespace APLPromoter.UI.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            var app = App.Current.Resources["Locator"] as ViewModelLocator;
            this.DataContext = app.ViewRouter;
            //this.DataContext = new APLPromoter.UI.Wpf.Views.ViewRouter(); //todo: work around bc using xaml data context, login does not show
        }
    }
}
