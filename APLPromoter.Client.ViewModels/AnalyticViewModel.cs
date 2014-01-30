using APLPromoter.Client.Contracts;
using APLPromoter.Client.Entity;
using APLPromoter.Core.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using APLPromoter.Core.Reactive;

namespace APLPromoter.Client.ViewModels
{
    public enum AnalyticStatus
    {
        Uninitialized,
        Checking,
        Loading,
        Applying,
        Completed
    }
    public class AnalyticViewModel : ViewModelBase, IContentSource
        //, IReactiveNotifyPropertyChanged
    {

        IAnalyticService _AnalyticProxy;
        IEventAggregator _EventAggregator;
        
        string _SqlKey = ConfigurationManager.AppSettings["SqlKey"].ToString();
        EditAnalyticViewModel _SelectedIdentityViewModel;

        private List<Analytic.Identity> _AnalyticIds = new List<Analytic.Identity>();

        public DelegateCommand<Analytic.Identity> EditAnalyticCommand { get; set; }
        public DelegateCommand<Analytic.Identity> AddAnalyticCommand { get; set; }
        public ReactiveCommand LoadFiltersCommand { get;  set; }
        public ReactiveCommand LoadIdentitiesCommand { get; set; }

        private AnalyticStatus status;
        
        private Analytic.Identity _CheckedIdentity;
        public Analytic.Identity CheckedIdentity { 
            get { return _CheckedIdentity; }
            set {
                _CheckedIdentity = value;
                OnPropertyChanged(() => CheckedIdentity);
            }
        }
        [ImportingConstructor]
        public AnalyticViewModel(IAnalyticService service, 
                                    IEventAggregator aggregator)
        {

            _AnalyticProxy = service;
            _EventAggregator = aggregator;
            
            EditAnalyticCommand = new DelegateCommand<Analytic.Identity>(OnEditAnalyticCommand);
            AddAnalyticCommand = new DelegateCommand<Analytic.Identity>(OnAddAnalyticCommmand);




        }



        ObservableAsPropertyHelper<List<APLPromoter.Client.Entity.Filter>> _Filters;
        //List<Client.Entity.Filter> _Filters;
        public List<APLPromoter.Client.Entity.Filter> Filters
        {
            get { return _Filters.Value; }
            //set { 
            //        _Filters = value;
            //        OnPropertyChanged(() => Filters);
            //    }
        }


        public List<Analytic.Identity> LoadIdentities()
        {
            var _AnalyticIds = new List<Client.Entity.Analytic.Identity>();
            UsingProxy<IAnalyticService>(_AnalyticProxy, proxy =>
            {
                var response = _AnalyticProxy.LoadList(
                            new Session<NullT>
                            {
                                SqlKey = ConfigurationManager.AppSettings["SqlKey"].ToString()
                            });
                if (response.Data != null)
                {
                    AnalyticIds.AddRange(response.Data);
                    _AnalyticIds.AddRange(response.Data);
                }
            });

            return _AnalyticIds;

        }

        //TODO: Async load?
        public List<Client.Entity.Filter> LoadFilters(Client.Entity.Analytic.Identity id){
            var list = new List<Client.Entity.Filter>();
            UsingProxy<IAnalyticService>(_AnalyticProxy, proxy =>
            {
                var response = proxy.LoadFilters( new Session<Client.Entity.Analytic.Identity>{
                                                    SqlKey = _SqlKey,
                                                    Data = id
                                                });
               if(response.Data != null)
               {
                   list.AddRange(response.Data);
               }

            });
            return list;
        }
        public EditAnalyticViewModel SelectedAnalytic {
            get { return _SelectedIdentityViewModel; }
            protected set {
                _SelectedIdentityViewModel = value;
                OnPropertyChanged(() => SelectedAnalytic);
            }
        }

        ObservableAsPropertyHelper<ReactiveList<APLPromoter.Client.Entity.Analytic.Identity>> _Identities;
        public ReactiveList<APLPromoter.Client.Entity.Analytic.Identity> Identities
        {
            get { return _Identities.Value; }

        }
        public List<Analytic.Identity> AnalyticIds
        {
            get { return _AnalyticIds; }
            set
            {
                if (_AnalyticIds != value)
                {
                    _AnalyticIds = value;
                    OnPropertyChanged(() => AnalyticIds, false);
                }
            }
        }
        
        protected override void OnViewLoaded()
        {

            

        }
        


        public void LoadTypes(){ }

        void OnEditAnalyticCommand(Analytic.Identity analytic)
        {
            if (analytic != null)
            {
                _SelectedIdentityViewModel = new EditAnalyticViewModel(_AnalyticProxy, analytic, _EventAggregator);
                
                
                _SelectedIdentityViewModel.AnalyticUpdated += SelectedIdentityViewModel_AnalyticUpdated;
                _SelectedIdentityViewModel.CancelEditAnalytic += SelectedIdentityViewModel_CancelEditAnalytic;
            }
        }

        void OnAddAnalyticCommmand(Analytic.Identity analytic)
        {
            Analytic.Identity id = new Analytic.Identity();
            _SelectedIdentityViewModel.AnalyticUpdated += SelectedIdentityViewModel_AnalyticUpdated;
            _SelectedIdentityViewModel.CancelEditAnalytic += SelectedIdentityViewModel_CancelEditAnalytic;

        }

        void SelectedIdentityViewModel_AnalyticUpdated(object sender, AnalyticEventArgs e) 
        {
            if(!e.IsNew)
            {
                Analytic.Identity selectedId = _AnalyticIds.Where(listItem => listItem.Id == e.AnalyticIdentity.Id).FirstOrDefault();
                if (selectedId != null)
                {
                    selectedId.Name = e.AnalyticIdentity.Name;
                    selectedId.Description = e.AnalyticIdentity.Description;
                }
            }
        }
        void SelectedIdentityViewModel_CancelEditAnalytic(object sender, EventArgs e) 
        {
            _SelectedIdentityViewModel = null;
        }

        //public IObservable<IObservedChange<object, object>> Changed
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public IObservable<IObservedChange<object, object>> Changing
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public IDisposable SuppressChangeNotifications()
        //{
        //    throw new NotImplementedException();
        //}

        //public event PropertyChangingEventHandler PropertyChanging;
    }


    public class EditAnalyticViewModel : ViewModelBase
    {
        IAnalyticService _AnalyticProxy;
        IEventAggregator _EventAggregator;
        Analytic.Identity _Analytic;


        private readonly Subject<EditAnalyticViewModel> _IdentitySelected = new Subject<EditAnalyticViewModel>();
        public IObservable<EditAnalyticViewModel> IdentitySelected { get { return _IdentitySelected.AsObservable(); } }

        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> CancelCommand { get; private set; }

        public EventHandler<AnalyticEventArgs> AnalyticUpdated;
        public EventHandler CancelEditAnalytic;

        public EditAnalyticViewModel(IAnalyticService service, Analytic.Identity analytic, IEventAggregator eventAggregator)
        {
            _AnalyticProxy = service;
            _Analytic = analytic;
            _EventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand<object>(OnSaveCommandExecute, OnSaveCommandCanExecute);
            CancelCommand = new DelegateCommand<object>(OnCancelCommandExecute, OnCancelCommandCanExecute);
        }

        void OnCancelCommandExecute(object arg)
        {
            if (CancelEditAnalytic != null)
            {
                CancelEditAnalytic(this, EventArgs.Empty);
            }
        }
        Boolean OnCancelCommandCanExecute(object arg)
        {
            return _Analytic.IsDirty;
        }


        public Analytic.Identity Analytic
        {
            get { return _Analytic; }
        }

        public void OnSaveCommandExecute(object arg) {
            ValidateModel();
            if (IsValid)
            {
                UsingProxy<IAnalyticService>(_AnalyticProxy, proxy => {
                    bool isNew = (_Analytic.Id == 0);
                    var response = proxy.SaveIdentity( 
                                new Session<Analytic.Identity>{ 
                                            Data = _Analytic,
                                            SqlKey = ConfigurationManager.AppSettings["SqlKey"].ToString()
                                });
                    if (!string.IsNullOrEmpty(response.ClientMessage) && !string.IsNullOrEmpty(response.ServerMessage))
                    {
                        //TODO: Use EventAggregator?
                        _EventAggregator.Publish(new AnalyticUpdatedEvent { AnalyticId = _Analytic.Id, Name = _Analytic.Name });
                        if (AnalyticUpdated != null)
                            AnalyticUpdated(this, new AnalyticEventArgs(response.Data, isNew));
                    

                    }
                    //else {} TODO: Display server validation messages
                });
                //TODO: Select this entity by default when saved successfully
                _IdentitySelected.OnNext(this);
            }
        }
        bool OnSaveCommandCanExecute(object arg) {
            return _Analytic.IsDirty;
        }
    }

    public class AnalyticEventArgs : EventArgs
    {
        public AnalyticEventArgs(Analytic.Identity identity, Boolean isNew)
        {
            this.AnalyticIdentity = identity;
            this.IsNew = isNew;
        }
        public Analytic.Identity AnalyticIdentity { get; set; }
        public bool IsNew { get; set; }
    }


    class AnalyticSelectedEvent
    {
        public int AnalyticId { get; set; }
        public string Name { get; set; }
    }

    class AnalyticUpdatedEvent
    {
        public int AnalyticId { get; set; }
        public string Name { get; set; }
    }
}
