using APLPromoter.Client.Contracts;
using APLPromoter.Client.Entity;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Linq;


namespace APLPromoter.UI.Wpf.ViewModel
{
    public class LoginViewModel : ReactiveObject , IRoutableViewModel
    {
        //APLPromoter.Client.UserClient _proxy = new APLPromoter.Client.UserClient();
        public LoginViewModel(IScreen hostScreen, MainViewModel appRootViewModel, 
                                IUserService userService)
        {
            HostScreen = hostScreen;
            //UserService = new UserService();
            UserService = userService;
            Application = appRootViewModel;

            var canLogin = this.WhenAny(x => x.LoginName, x => x.Password, (l, p) =>
                !String.IsNullOrWhiteSpace(l.Value) && !String.IsNullOrWhiteSpace(p.Value));
            
            LoginCommand = new ReactiveCommand(canLogin);
            ChangePasswordCommand = new ReactiveCommand(canLogin);
            
            //Reset to initialize view
            SplashVisible = Visibility.Visible; //TODO: Add bool to visibility converter
            InitializationMessage = "Initializing...";
            LoginVisible = Visibility.Collapsed;
            IsProgressRunning = true;
            

            
            //Steps
            LoginSteps = new List<Step> {
                new Step{ StepType = Steps.Initialization, Caption = "Initalizing", IsValid = false, },
                new Step{ StepType = Steps.Authentication, Caption = string.Empty, IsValid = false },
                new Step{ StepType = Steps.PasswordChange, Caption = string.Empty, IsValid = false}
            };
            
           

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
                                    ToggleStepStatus(Steps.Initialization, false, "Connection failed. Try again?");
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
                                ToggleStepStatus(Steps.Initialization, false, "Connection failed. Try again?");
                                IsProgressRunning = false;

                            }
                            
                        }, _currentScheduler);


            //handle initialization errors
            initialized.ContinueWith(t => Console.WriteLine("test"), TaskContinuationOptions.OnlyOnFaulted);



            var loggedIn = LoginCommand.RegisterAsyncTask(async _ =>
                            {
                                Message = "Authenticating....";
                                ToggleStepStatus(Steps.Authentication, false, "Authenticating...");
                                var session = await Authenticate(LoginName, Password);
                                //var message = await InitializeAndLoginAsync();

                                if (session.Authenticated)
                                {
                                    HostScreen.Router.Navigate.Execute(Application);
                                }
                                else
                                {
                                    Message = "Login Failed";
                                    ToggleStepStatus(Steps.Authentication, false, "Login failed.");
                                    Password = string.Empty;
                                    LoginName = LoginName;
                                }
                            });

            var changingPassword = ChangePasswordCommand.RegisterAsyncTask(async _ =>
                {
                            var authResponse = await Authenticate(LoginName, Password);

                            if (authResponse.Authenticated)
                            {
                                ToggleStepStatus(Steps.Authentication, true, "Login Successful.");
                                    
                                var changeResponse = await ChangePassword(LoginName, Password, 
                                                 NewPassword);
                                if(changeResponse.SessionOk)
                                {
                                    ToggleStepStatus(Steps.Authentication, true, "Password Successfully Changed");
                                }
                            }

                 });

            ChangePasswordCommand.ThrownExceptions.Subscribe(ex => {
                
                //Logout
                //Reset steps

            });
     }

        public void ToggleStepStatus(Steps step, bool isValid, string caption)
        {
            var st = LoginSteps.FirstOrDefault(s => s.StepType == step);
            st.IsValid = isValid;
            st.Caption = caption;
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

        public async Task<Session<NullT>> Authenticate(string loginName, string password)
        {
            return await Task.Run(() =>
            {

                var authenticated = false;

                Session.UserIdentity = new User.Identity
                {
                    Login = loginName,
                    Password = new User.Password { Old = Password }
                };



                return UserService.Authenticate(Session); //TODO:Using and Dispose of proxy.


            });
        }

        public async Task<Session<NullT>> ChangePassword(string loginName, string password, string newPassword)
        {
            return await Task.Run(() =>
            {

                try
                {
                    
                    Session.UserIdentity = new User.Identity
                    {
                        Login = loginName,
                        Password = new User.Password { Old = Password, New = newPassword }
                    };
                    return UserService.SavePassword(Session);
                }
                catch (Exception)
                {
                    throw;
                }

            });
        }

        List<Step> _steps;
        public List<Step> LoginSteps
        {
            get
            {
                return _steps;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _steps, value);
            }
        }

        Session<NullT> _session;
        public Session<NullT> Session
        {
            get
            { return _session; }
            set { this.RaiseAndSetIfChanged(ref _session, value); }
        } //TODO: move to ViewModelLocator
        

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

                return IsProgressRunning ? (SolidColorBrush)(new BrushConverter()
                    .ConvertFrom("#FFABABAB")) : Brushes.Red; 
            
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

        string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set { this.RaiseAndSetIfChanged(ref _newPassword, value); }
        }

        string _confirmPassword;
        public string confirmPassword
        {
            get { return _newPassword; }
            set { this.RaiseAndSetIfChanged(ref _confirmPassword, value); }
        }




        



        public ReactiveCommand LoginCommand { get; set; }
        public ReactiveCommand ChangePasswordCommand { get; set; }

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





