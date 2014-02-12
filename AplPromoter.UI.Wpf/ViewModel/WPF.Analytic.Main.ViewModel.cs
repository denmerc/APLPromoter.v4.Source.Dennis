using APLPromoter.Client.Contracts;
//using APLPromoter.UI.Wpf.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace APLPromoter.UI.Wpf.ViewModel
{
    public class AnalyticViewModel : ReactiveObject, IRoutableViewModel
    {
        public AnalyticViewModel(Analytic analytic)
        {
            Model = analytic;
            if (Model != null)
            {
                this.Name = Model.Name;
                this.HashCode = this.GetHashCode();
            }

            var filtersCmd = new ReactiveCommand(null);
            var pushFilters = filtersCmd.RegisterAsyncFunction(filters => LoadFilters())
                .Subscribe(x =>
                {
                    Filters.Clear();
                    Filters.AddRange(x);
                });
            
            var typesCmd = new ReactiveCommand(null);
            var pushTypes = typesCmd.RegisterAsyncFunction(types => LoadTypes())
                .Subscribe(x =>
                {
                    Types.Clear();
                    Types.AddRange(x);
                });

            filtersCmd.Execute(null);
            typesCmd.Execute(null);

            var steps = new List<ReactiveObject>()
            {
                new EditIdentityViewModel(),
                new EditFiltersViewModel()
            };
            Steps = steps;


        }

        public IAnalyticService AnalyticService { get; set; }
        public ReactiveCommand FiltersCommand { get; set; }
        public ReactiveCommand TypesCommand { get; set; }
        private ReactiveList<Type> _Types = new ReactiveList<Type>();
        public ReactiveList<Type> Types
        {
            get
            {
                return this._Types;
            }
            set
            {
                if (_Types != value)
                {
                    _Types = value;
                    this.RaiseAndSetIfChanged(ref _Types, value);
                }
            }
        }

        private ReactiveList<Filter> _Filters = new ReactiveList<Filter>();
        public ReactiveList<Filter> Filters
        {
            get
            {
                return this._Filters;
            }
            set
            {
                if (_Filters != value)
                {
                    _Filters = value;
                    this.RaiseAndSetIfChanged(ref _Filters, value);
                }
            }
        }

        private int hashCode;
        public int HashCode
        {
            get
            {
                return this.GetHashCode();
            }
            set
            {
                if (hashCode != value)
                {
                    hashCode = value;
                    this.RaiseAndSetIfChanged(ref hashCode, value);
                }
            }
        }
        private Analytic _Model;
        public Analytic Model 
        {
            get
            {
                return _Model;
            }
            set
            {
                if (_Model != value)
                {
                    _Model = value;
                    this.RaiseAndSetIfChanged(ref _Model, value);
                }
            }
        }


        private int _id;
        public int Id
        {
            get
            {
                if (Model != null)
                    return Model.Id;
                else return 0;
            }
            set
            {
                if (Model != null || _id != value)
                {
                    _Model.Id = value;
                    this.RaiseAndSetIfChanged(ref _id, value);
                }
            }
        }
        private string _Name;
        public string Name
        {
            get
            {
                if (Model != null)
                    return Model.Name;
                else return null;
            }
            set
            {
                if (Model != null || _Name != value )
                {
                    _Model.Name= value;
                    this.RaiseAndSetIfChanged(ref _Name, value);
                }
            }
        }


        private string _Description;
        public string Description
        {
            get
            {
                if (Model != null)
                    return Model.Description;
                else return null;
            }
            set
            {
                if (Model != null || _Description != value)
                {
                    _Model.Description = value;
                    this.RaiseAndSetIfChanged(ref _Description, value);
                }
            }
        }
        private List<ReactiveObject> Steps { get; set; } //TODO: EditIdentity, Filters, Types VMs
        public ReactiveObject CurrentStep { get; set; }
        public ReactiveCommand NextStep { get; set; }
        public ReactiveCommand PreviousStep { get; set; }
        public ReactiveCommand Save { get; set; }
        public ReactiveCommand Cancel { get; set; }
        private IRoutingState WizardNavigator { get; set; }

        public IScreen HostScreen { get; set; }
        public IRoutingState Router { get; set; }
        public string UrlPathSegment { get { return "AnalyticEdit"; } }

        public ObservableCollection<Filter> LoadFilters(){
            return new ObservableCollection<Filter>() {
                new Filter { Id = 1, Name = "Filter1", Description = "Filter Description 1"},
                new Filter { Id = 2, Name = "Filter2", Description = "Filter Description 2"},
                new Filter { Id = 3, Name = "Fitler3", Description = "Filter Description 3"}
            };
        }

        public ObservableCollection<Type> LoadTypes()
        {
            return new ObservableCollection<Type>() {
                new Type { Id = 1, Name = "Types1", Description = "Types Description 1"},
                new Type { Id = 2, Name = "Types2", Description = "Types Description 2"},
                new Type { Id = 3, Name = "Types3", Description = "Types Description 3"}
            };
        }


        public class EditIdentityViewModel: ReactiveObject
        {
            public EditIdentityViewModel() { }
        }
        public class EditFiltersViewModel : ReactiveObject
        {
            
        }
    }




}
