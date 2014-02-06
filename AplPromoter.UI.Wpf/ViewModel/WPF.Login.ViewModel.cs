using APLPromoter.Client.Contracts;
using APLPromoter.Client.Entity;
using APLPromoter.Core.UI;
using Promoter.Services;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Configuration;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;
using System.Windows.Media;

namespace APLPromoter.UI.Wpf.ViewModel
{
    public class LoginViewModel : ReactiveObject , IRoutableViewModel
    {
        //APLPromoter.Client.UserClient _proxy = new APLPromoter.Client.UserClient();
        public LoginViewModel(IScreen hostScreen , MainViewModel appRootViewModel, IUserService userService)
        {
            HostScreen = hostScreen;
            //UserService = new UserService();
            UserService = userService;
            Application = appRootViewModel;

            var canLogin = this.WhenAny(x => x.LoginName, x => x.Password, (l, p) =>
                !String.IsNullOrWhiteSpace(l.Value) && !String.IsNullOrWhiteSpace(p.Value));

            LoginCommand = new ReactiveCommand(canLogin);
            
            //Reset to initialize view
            SplashVisible = Visibility.Visible; //TODO: Add bool to visibility converter
            InitializationMessage = "Initializing...";
            LoginVisible = Visibility.Collapsed;
            IsProgressRunning = true;
            

            TaskScheduler _currentScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Task<Session<NullT>> initialized = Initialize();

            initialized.ContinueWith(t =>
                        {
                            try
                            {
                               
                                
                                Session = t.Result ;
                                if (Session == null || Session.AppOnline == false) //invalid session prompt to reconnect
                                {
                                    InitializationMessage = "Connection failed"; //send failed event message
                                    IsProgressRunning = false;
                                    ProgressBrush = Brushes.Red;
                                }
                                else 
                                { 
                                    //Switch to login
                                    SplashVisible = Visibility.Collapsed;
                                    LoginVisible = Visibility.Visible;
                                }

                            }
                            catch (Exception ex)
                            {

                                InitializationMessage = "Connection failed.";
                                IsProgressRunning = false;
                                
                                
                            }
                            
                        }, _currentScheduler);



            var loggedIn = LoginCommand.RegisterAsyncTask(async _ =>
                            {
                                

                                Message = "Authenticating....";
                                var authenticated = await Authenticate(LoginName, Password);
                                //var message = await InitializeAndLoginAsync();

                                if (authenticated)
                                {
                                    HostScreen.Router.Navigate.Execute(Application);
                                }
                                else
                                {
                                    Message = "Login Failed";
                                    Password = string.Empty;
                                    LoginName = LoginName;
                                }
                                


                            });


            
        }


        public async Task<Session<NullT>> Initialize()
        {
            return await Task.Run(() =>
            {
                Session<NullT> key = new Session<NullT>()
                {
                    SqlKey = ConfigurationManager.AppSettings["sharedKey"].ToString()
                };
                return UserService.Initialize(key);
            });
        }

        public async Task<bool> Authenticate(string loginName, string password)
        {
            return await Task.Run(() =>
            {

                var authenticated = false;

                Session.UserIdentity = new User.Identity
                {
                    Login = loginName,
                    Password = new User.Password { Old = Password }
                };


                return authenticated = UserService.Authenticate(Session).Authenticated; //TODO:Using and Dispose of proxy.


            });
        }

        public async Task<string> InitializeAndLoginAsync()
        {
            var initialized = await Task.Run(() =>
            {
                Session<NullT> key = new Session<NullT>()
                {
                    SqlKey = System.Configuration.ConfigurationManager.AppSettings["sharedkey"].ToString()
                };
                return UserService.InitializeAsync(key);

            });


            var authenticated = false;
            if (initialized.SessionOk)
            {
                initialized.UserIdentity = new User.Identity
                {
                    Login = LoginName,
                    Password = new User.Password { Old = Password }
                };


                authenticated = await Task.Run(() =>
                {
                    return UserService.Authenticate(initialized).Authenticated;
                });

            }

            return authenticated ? "Login Succeeded" : "Login Failed";

        }


        public async Task<string> InitializeAndLoginAsyncNoContext()
        {
            var initialized = await Task.Run(() =>
            {
                Session<NullT> key = new Session<NullT>()
                {
                    SqlKey = System.Configuration.ConfigurationManager.AppSettings["sharedkey"].ToString()
                };
                return UserService.InitializeAsync(key);

            }).ConfigureAwait(false);


            var authenticated = false;
            if (initialized.SessionOk)
            {
                initialized.UserIdentity = new User.Identity
                {
                    Login = LoginName,
                    Password = new User.Password { Old = Password }
                };


                authenticated = await Task.Run(() =>
                {
                    return UserService.Authenticate(initialized).Authenticated;
                }).ConfigureAwait(false);

            }

            return authenticated ? "Login Succeeded" : "LoginCommand Failed";

        } 

        Session<NullT> _session;
        public Session<NullT> Session
        {
            get
            { return _session; }
            set { this.RaiseAndSetIfChanged(ref _session, value); }
        }
        

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


        Visibility _loginVisible;
        public Visibility LoginVisible
        {
            get
            { return _loginVisible; }
            set { this.RaiseAndSetIfChanged(ref _loginVisible, value); }
        }

        Visibility _splashVisible;
        public Visibility SplashVisible
        {
            get
            { return _splashVisible; }
            set { this.RaiseAndSetIfChanged(ref _splashVisible, value); }
        }

        bool _isProgessRunning;
        public bool IsProgressRunning
        {
            get
            { return _isProgessRunning; }
            set { this.RaiseAndSetIfChanged(ref _isProgessRunning, value); }
        }

        Brush _progressBrush;
        public Brush ProgressBrush
        {
            get
            { 

                return IsProgressRunning ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFABABAB")) : Brushes.Red; 
            
            }
            set { this.RaiseAndSetIfChanged(ref _progressBrush, value); }
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

        string message;
        public string Message
        {
            get { return message; }
            set { this.RaiseAndSetIfChanged(ref message, value); }
        }

        string _initializationMessage;
        public string InitializationMessage
        {
            get { return _initializationMessage; }
            set { this.RaiseAndSetIfChanged(ref _initializationMessage, value); }
        }

        public string UrlPathSegment
        {
            get { return "login"; }
        }


    }
}
