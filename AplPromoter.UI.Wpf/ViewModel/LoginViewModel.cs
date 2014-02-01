using APLPromoter.Client.Contracts;
using APLPromoter.Client.Entity;
using Promoter.Services;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Configuration;

namespace APLPromoter.UI.Wpf.ViewModel
{
    public class LoginViewModel : ReactiveObject , IRoutableViewModel
    {
        APLPromoter.Client.UserClient _proxy = new APLPromoter.Client.UserClient();
        public LoginViewModel(IScreen hostScreen , MainViewModel appRootViewModel, IUserService userService)
        {
            HostScreen = hostScreen;
            //UserService = new UserService();
            UserService = userService;
            
            Application = appRootViewModel;

            var canLogin = this.WhenAny(x => x.LoginName, x => x.Password, (l, p) =>
                !String.IsNullOrWhiteSpace(l.Value) && !String.IsNullOrWhiteSpace(p.Value));

            LoginCommand = new ReactiveCommand(canLogin);


            //Session<NullT> test = new Session<NullT>
            //{
            //    SqlKey = "72B9ED08-5D12-48FD-9CF7-56A3CA30E660"

            //};
            //LoginCommand  = new APLPromoter.Core.UI.DelegateCommand<Session<NullT>>(OnLogin);

            //Observable.Using(() => new APLPromoter.Client.UserClient(),
            //    c => c.Initialize(test).);
            //            //.Subscribe(response => { 
            //            //    if(response.)
                        
            //            //});



 

            var loggedIn = LoginCommand.RegisterAsync(_ => Observable.Start(() =>
                {


                    Session<NullT> init = new Session<NullT>
                    {
                        SqlKey = System.Configuration.ConfigurationManager.AppSettings["sharedKey"].ToString()
                                        
                    };

                    var initResponse = _proxy.Initialize(init);
                    var authenticationResult = false;
                    if (initResponse.SessionOk)
                    {
                        initResponse.UserIdentity = new User.Identity
                        {
                            Login = LoginName,
                            Password = new User.Password { Old = Password }
                        };

                        
                        authenticationResult = _proxy.Authenticate(initResponse).Authenticated;
                        return authenticationResult ? "Login Succeeded...Continuing to Analytics"
                            : "Login Failed...Please try again";
                    }
                    else return "Failed to Initialize.";

                    
                    //var AuthenticationResult = UserService.Authenticate(LoginName, Password);
                    //return AuthenticationResult == AuthenticationResult.Authenticated
                    //    ? "Login Succeeded...Continuing to Analytics"
                    //    : "Login Failed...Please try again.";
                }));


            loggedIn.Subscribe(s =>
            {
                if (s == "Login Succeeded...Continuing to Analytics")
                {
                    HostScreen.Router.Navigate.Execute(Application);
                }

            });


            message = loggedIn.ToProperty(this, x => x.Message, string.Empty);
            //message = new ObservableAsPropertyHelper<string>(loggedIn,
            //    s =>
            //    {

            //        raisePropertyChanged("Message");

            //    });
        }



        //public void OnLogin(Session<NullT> session)
        //{

        //    Message = string.Empty;
        //    var initResponse = _proxy.Initialize(session);
        //    var authenticationResult = false;
        //    if (initResponse.SessionOk)
        //    {
        //        initResponse.UserIdentity = new User.Identity
        //        {
        //            Login = LoginName,
        //            Password = new User.Password { Old = Password }
        //        };
        //    }

        //    authenticationResult = _proxy.Authenticate(initResponse).Authenticated;

        //    if (authenticationResult)
        //        HostScreen.Router.Navigate.Execute(Application);
        //    else Message = "Login Failed.";
        //}


        IUserService _userService;
        public IUserService UserService
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
        //public APLPromoter.Core.UI.DelegateCommand<Session<NullT>> LoginCommand { get; set; }


        public IScreen HostScreen { get; private set; }

        //string message;
        //public string Message
        //{
        //    get { return message2; }
        //    set { this.RaiseAndSetIfChanged(ref message2, value); }
        //}

        ObservableAsPropertyHelper<string> message;
        public string Message
        {

            get
            {
                return message.Value;
            }
        }

        public string UrlPathSegment
        {
            get { return "login"; }
        }


    }
}
