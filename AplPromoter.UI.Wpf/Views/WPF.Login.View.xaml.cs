using APLPromoter.UI.Wpf.ViewModel;
using ReactiveUI;
using System.Windows;

namespace APLPromoter.UI.Wpf.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : IViewFor<LoginViewModel>
    {
       public LoginView()
        {
            InitializeComponent();
            
            
            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);
            this.Bind(ViewModel, model => model.Password, x => x.password.Text);
            this.Bind(ViewModel, model => model.LoginName, view => view.userName.Text);
            this.OneWayBind(ViewModel, model => model.Message, x => x.message.Content);
            this.OneWayBind(ViewModel, model => model.InitializationMessage, x => x.xInitializationMessage.Content);
            this.OneWayBind(ViewModel, x => x.LoginCommand, x => x.login.Command);

            this.OneWayBind(ViewModel, model => model.IsProgressRunning, x => x.xProgress.IsIndeterminate);
            //this.OneWayBind(ViewModel, model => model.ProgressBrush, x => x.xProgress.Foreground);
            this.Bind(ViewModel, vm => vm.SplashVisible, view => view.xSplash.Visibility);
            this.Bind(ViewModel, vm => vm.LoginVisible, view => view.xLogin.Visibility);

        }

        public static readonly DependencyProperty ViewModelProperty =
    DependencyProperty.Register("ViewModel", typeof(LoginViewModel), typeof(LoginView), new PropertyMetadata(null));


        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (LoginViewModel)value; }
        }

        public LoginViewModel ViewModel
        {
            get
            {
                return (LoginViewModel)GetValue(ViewModelProperty);
            }
            set
            {
                SetValue(ViewModelProperty,
                    value);
            }
        }

    }
    
}
