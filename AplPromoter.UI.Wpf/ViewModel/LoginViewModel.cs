using Promoter.Services;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace APLPromoter.UI.Wpf.ViewModel
{
    public class LoginViewModel : ReactiveObject , IRoutableViewModel
    {
        public LoginViewModel(IScreen hostScreen , MainViewModel appRootViewModel, IAuthentication userService)
        {
            HostScreen = hostScreen;
            UserService = userService ?? new UserService();
            Application = appRootViewModel;

            var canLogin = this.WhenAny(x => x.LoginName, x => x.Password, (l, p) =>
                !String.IsNullOrWhiteSpace(l.Value) && !String.IsNullOrWhiteSpace(p.Value));

            LoginCommand = new ReactiveCommand(canLogin);

            var loggedIn = LoginCommand.RegisterAsync(_ => Observable.Start(() =>
                {
                    var AuthenticationResult = UserService.Authenticate(LoginName, Password);
                    return AuthenticationResult == AuthenticationResult.Authenticated
                        ? "Login Succeeded...Continuing to Analytics"
                        : "Login Failed...Please try again.";
                }));

            loggedIn.Subscribe(s =>
            {
                if (s == "Login Succeeded...Continuing to Analytics")
                {
                    HostScreen.Router.Navigate.Execute(Application);
                }
            });

            message = new ObservableAsPropertyHelper<string>(loggedIn,
                s => raisePropertyChanged("Message"));
        }


        IAuthentication _userService;
        public IAuthentication UserService
        {
            get
            { return _userService; }
            set { this.RaiseAndSetIfChanged(ref _userService, value); }
        }

        MainViewModel _application;
        public MainViewModel Application
        {
            get
            { return _application; }
            set { this.RaiseAndSetIfChanged(ref _application, value); }
        }


        string _loginName;
        public string LoginName
        {
            get
            { return _loginName; }
            set { this.RaiseAndSetIfChanged(ref _loginName, value); }
        }

        string _password;
        public string Password
        {
            get { return _password;}
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }

        public ReactiveCommand LoginCommand { get; set; }
        public IScreen HostScreen { get; private set; }

        ObservableAsPropertyHelper<string> message;
        public string Message
        {
            get { return message.Value; }
        }

        public string UrlPathSegment
        {
            get { return "login"; }
        }


    }
}
