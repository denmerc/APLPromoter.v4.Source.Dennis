using System.Windows;

namespace APLPromoter.UI.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var locator = new ViewModel.ViewModelLocator();
            App.Current.Resources.Add("Locator", locator);
        }
    }
}
