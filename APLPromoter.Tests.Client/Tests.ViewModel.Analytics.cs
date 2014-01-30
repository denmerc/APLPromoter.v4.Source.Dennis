using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using APLPromoter.Client.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using APLPromoter.Client.Contracts;
using APLPromoter.Client.Entity;
using FluentAssertions;
using ReactiveUI;
//using Microsoft.Reactive.Testing;
using System.Reactive.Linq;

using APLPromoter.Tests.Client.Setup;
using APLPromoter.Core.Reactive;


namespace APLPromoter.Tests.Client
{
    [TestClass]
    public class ViewModelTests
    {   
        
        IAnalyticService _analyticProxy;
        IEventAggregator _eventAggregator;
        [TestInitialize]
        public void Setup()
        {
            
            _analyticProxy = A.Fake<IAnalyticService>();
            _eventAggregator = new Core.Reactive.EventAggregator();
            //var _eventAggregator = A.Fake<IEventAggregator>();
            //var _windowManager = A.Fake<IWindowManager>();

        }

        #region Edit Identity
        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenUserInAnalytics_WhenViewLoaded_SameListOfAnalyticsDisplayed() 
        {

            Session<List<Analytic.Identity>> session = new Session<List<Analytic.Identity>>();
            List<Analytic.Identity> ids = new List<Analytic.Identity>()
            {
                new Analytic.Identity{ Name = "Name1", Description= "Description1"},
                new Analytic.Identity{ Name = "Name2", Description= "Description2"},
                new Analytic.Identity{ Name = "Name3", Description= "Description3"},
                new Analytic.Identity{ Name = "Name4", Description= "Description4"},
                new Analytic.Identity{ Name = "Name5", Description= "Description5"}
            };

            session.Data = ids;

            Session<NullT> nullSession = A.Fake<Session<NullT>>();
            AnalyticViewModel vm = new AnalyticViewModel(_analyticProxy, _eventAggregator);
            A.CallTo(() => _analyticProxy.LoadList(A<Session<NullT>>.Ignored)).Returns(session);

            vm.AnalyticIds.ShouldBeEquivalentTo(null, "PreLoad state not ids before view loaded");

            object loaded = vm.ViewLoaded; // fire off view loaded

            A.CallTo(() => _analyticProxy.LoadList(A<Session<NullT>>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            vm.AnalyticIds.Should().NotBeNull().And
                .HaveSameCount(ids).And
                .Equal(ids);


        }


        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenEditIdentity_WhenEditedCorrectly_IdentityIsValid(){
            Analytic.Identity id = new Analytic.Identity
            {
                Description = "Original Name",
                Name = "Original Description"
            };
            
            EditAnalyticViewModel vm = new EditAnalyticViewModel(_analyticProxy, id, _eventAggregator);

            vm.Analytic.IsValid.Should().BeTrue();
        }

        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenEditIdentity_WhenNameEmpty_IdentityIsValid()
        {
            Analytic.Identity id = new Analytic.Identity
            {
                Name = "",
                Description = "Original Name"
            };

            EditAnalyticViewModel vm = new EditAnalyticViewModel(_analyticProxy, id, _eventAggregator);

            vm.Analytic.IsValid.Should().BeFalse();
        }

        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenEditIdentity_WhenNameNull_IdentityIsValid()
        {
            Analytic.Identity id = new Analytic.Identity
            {
                Name = null,
                Description = "Original Name"
            };

            EditAnalyticViewModel vm = new EditAnalyticViewModel(_analyticProxy, id, _eventAggregator);

            vm.Analytic.IsValid.Should().BeFalse();
        }

        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenEditIdentity_WhenTooShortDescription_IdentityIsValid()
        {
            Analytic.Identity id = new Analytic.Identity
            {
                Name = "original name",
                Description = "a"
            };

            EditAnalyticViewModel vm = new EditAnalyticViewModel(_analyticProxy, id, _eventAggregator);

            vm.Analytic.IsValid.Should().BeFalse();
        }

        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenEditIdentity_WhenTooLongDescription_IdentityIsValid()
        {
            Analytic.Identity id = new Analytic.Identity
            {
                Name = "original name",
                Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pa"
            };

            EditAnalyticViewModel vm = new EditAnalyticViewModel(_analyticProxy, id, _eventAggregator);

            vm.Analytic.IsValid.Should().BeFalse();
        }

        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenEditIdentity_WhenDescriptionChanged_IdentityIsDirty()
        {
            Analytic.Identity id = new Analytic.Identity
            {
            };

            EditAnalyticViewModel vm = new EditAnalyticViewModel(_analyticProxy, id, _eventAggregator);

            id.Description = "updated desc";

            vm.Analytic.IsDirty.Should().BeTrue();
        }

        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenEditIdentity_WhenDescriptionUNchanged_IdentityIsDirty()
        {
            Analytic.Identity id = new Analytic.Identity
            {
            };

            EditAnalyticViewModel vm = new EditAnalyticViewModel(_analyticProxy, id, _eventAggregator);


            vm.Analytic.IsDirty.Should().BeFalse();
        }



        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenEditIdentity_WhenEditedCorrectly_IdentityCanBeSaved()
        {
            Analytic.Identity id = new Analytic.Identity
            {
                Description = "orig Description",
                Name = "Orig Name",
                IsDirty = false
            };


            EditAnalyticViewModel vm = new EditAnalyticViewModel(_analyticProxy, id, _eventAggregator);
            
            vm.Analytic.IsDirty.Should().BeFalse();
            vm.SaveCommand.CanExecute(null).Should().Equals(false);

            vm.Analytic.Description = "updated description";
            vm.SaveCommand.CanExecute(null).Should().Equals(true);
        }

        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenEditIdentity_WhenEditedCorrectly_ThenIdentitySaved()
        {
            
            //arrange
            Analytic.Identity id = new Analytic.Identity
            {
                Description = "orig Description",
                Name = "Orig Name"
            };

            EditAnalyticViewModel vm = new EditAnalyticViewModel(_analyticProxy, id, _eventAggregator);
            A.CallTo( () => _analyticProxy.SaveIdentity(A<Session<Analytic.Identity>>.Ignored))
                .Returns(new Session<Analytic.Identity> { 
                    Data = vm.Analytic, 
                    ClientMessage = "OK",
                    ServerMessage = "OK",
                });
            vm.Analytic.Description = "updated description"; //simulate user modifying

            bool isIdentityUpdated = false;
            string newDescription = string.Empty;
            
            //subscribe to updated event 
            vm.AnalyticUpdated += (s, e) =>
            {
                isIdentityUpdated = true;
                newDescription = e.AnalyticIdentity.Description;
            };


            //act
            vm.SaveCommand.Execute(null);

            //assert
            isIdentityUpdated.Should().BeTrue();
            newDescription.Should().Be("updated description");

        }

        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenEditIdentity_WhenEdited_ThenCanBeCancelled() 
        {
            Analytic.Identity id = new Analytic.Identity
            {
                Description = "orig Description",
                Name = "Orig Name"
            };

            EditAnalyticViewModel vm = new EditAnalyticViewModel(_analyticProxy, id, _eventAggregator);

            Boolean isCancelled = false;
            vm.CancelEditAnalytic += (s, e) => isCancelled = true;
            vm.CancelCommand.Execute(null);

            isCancelled.Should().BeTrue();
            
        }


        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenEditIdentity_WhenEdited_ThenIsCancelled()
        {
            Analytic.Identity id = new Analytic.Identity
            {
                Description = "orig Description",
                Name = "Orig Name"
            };

            EditAnalyticViewModel vm = new EditAnalyticViewModel(_analyticProxy, id, _eventAggregator);
            vm.CancelCommand.CanExecute(null).Should().BeTrue();

        }
        #endregion


        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenAnalyticList_WhenExceptionOccurs_UserIsNotifiedAppropriately()
        {

        }


        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenAnalyticList_WhenLoadTimeIsSlow_UserIsDisplayedLoadingMessage()
        {


        }


        #region Wizard Interaction with Dashboard concept


        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenAnalyticListLoaded_WhenAnyAnalyticIsSelected_FilterNextButtonEnabled()
        {

            //Create mainviewmodel representing sections of app - dashboard, menu footer

            var aVM = new AnalyticViewModel(_analyticProxy, _eventAggregator);

            var dvm = new DashboardViewModel(_eventAggregator);

            MainViewModel vm = new MainViewModel(
                    aVM,
                    dvm,
                    new TreeViewModel(),
                    new MenuViewModel()
                );

            //fake id list
            var ids = new List<Analytic.Identity>{
                new Analytic.Identity{
                    Id = 0,
                    Name = "Name1",
                    Description = "Desc1"
                },
                new Analytic.Identity{
                    Id = 1,
                    Name = "Name2",
                    Description = "Desc2"
                }

            };

            //fake LoadIdentity
            A.CallTo(() => _analyticProxy.LoadList(A<Session<NullT>>.Ignored)).Returns(new Session<List<Analytic.Identity>> { Data = ids });

            //setup Load of analytics and selected event
            var loadIdsCmd = new ReactiveCommand(null);
            //var load = loadIdsCmd.RegisterAsyncFunction<List<Analytic.Identity>>(_ => aVM.LoadIdentities());
            var load = loadIdsCmd.RegisterAsync(_ => Observable.Start(() => aVM.LoadIdentities()));

            var rlist = new ReactiveList<SelectableViewModel>();
            loadIdsCmd.Subscribe(list =>
                {

                    rlist.Clear();
                    rlist.AddRange(aVM.LoadIdentities().Select(d => new SelectableViewModel(d.Id)));
                    //rlist = aVM.AnalyticIds.CreateDerivedCollection(x => new SelectableViewModel(x));

                }
                
            );
            aVM.LoadIdentitiesCommand = loadIdsCmd;
            aVM.LoadIdentitiesCommand.Execute(null);


            //watch isChecked and uncheck the others - updating checkedIdentity
            rlist.ChangeTrackingEnabled = true;
            var futureCheckedIds = rlist.ItemChanged.Subscribe(c =>
                {
                    var sender = c.Sender;
                    if (sender.IsChecked == true) {
                        aVM.CheckedIdentity = aVM.AnalyticIds.Where(x => x.Id == c.Sender.Id).FirstOrDefault();
                        
                        foreach (var item in rlist)
                        {
                            if (item.Id != c.Sender.Id)
                                item.IsChecked = false;
                        }
                    }
                
                }
            );

            //enable FiltersCommand based on if anything is selected.
            var canExecuteFilters = aVM.ObservableForProperty(v => v.CheckedIdentity).Select( x => x.Value != null);
            var filterCmd = new ReactiveCommand(canExecuteFilters, false);

            //Act
            //fire off test selection event

            if (rlist != null && rlist.Count >= 1)
            {
                rlist.First().IsChecked = true;
            }


            //Assert 
            //next filter command is enabled.
            filterCmd.CanExecute(null).Should().BeTrue();
            //id is first checked
            aVM.CheckedIdentity.Id.Should().Equals(0);
            
            //Load Filters
            //vm.ContentPanel.As<AnalyticViewModel>().AddAnalyticCommand.Execute(null);


        }

        public class SelectableViewModel:ReactiveObject{
            public Int32 Id { get; set; }

            Boolean _IsChecked;
            public Boolean IsChecked {
                get { return _IsChecked; } 
                set {this.RaiseAndSetIfChanged(ref _IsChecked, value);}
            }
            public SelectableViewModel(int id){
                this.Id = id;
            }
        }

        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenAnalyticWizardStarted_WhenFiltersAreSelected_DashboardDisplaysValidations()
        {
            //(new TestScheduler()).With(sched => { 
                
            //    //sample filter data
            //    var filterObservable = Observable.Return(CreateSampleData())
            //        .Delay(TimeSpan.FromMilliseconds(5000), RxApp.TaskpoolScheduler);

            //    //fake command to mimic service call returning sample filters
            //    var command  = new ReactiveCommand().RegisterAsync<List<Filter.Value>>(x => filterObservable);


                    
            
            
            //});
        }

        [TestMethod, TestCategory("Event Aggregrator")]
        public void EventAggregator_SelectedEvent_Subscribe()
        {
            var eventWasRaised = false;
            var eventPublisher = new Core.Reactive.EventAggregator();

            eventPublisher.GetEvent<AnalyticSelectedEvent>().Subscribe( e => eventWasRaised = true);

            eventPublisher.Publish(new AnalyticSelectedEvent { AnalyticId = 1, Name = "Analytic Name 1" });

            eventWasRaised.Should().BeTrue();

        }

        [TestMethod, TestCategory("Event Aggregrator")]
        public void EventAggregator_SelectedEvent_UnSubscribe()
        {
            var eventWasRaised = false;
            var eventPublisher = new Core.Reactive.EventAggregator();

            var subscription = eventPublisher.GetEvent<AnalyticSelectedEvent>().Subscribe(e => eventWasRaised = true);

            subscription.Dispose();
            eventPublisher.Publish(new AnalyticSelectedEvent { AnalyticId = 1, Name = "Analytic Name 1" });

            eventWasRaised.Should().BeFalse();

        }


        [TestMethod, TestCategory("Event Aggregrator")]
        public void EventAggregator_SelectedEvent_WithnotFoundCriteria_SubscriptionIsIgnored()
        {
            var eventWasRaised = false;
            var eventPublisher = new Core.Reactive.EventAggregator();

            eventPublisher.GetEvent<AnalyticSelectedEvent>()
                .Where( e => e.Name == "Wrong Name 1")
                .Subscribe(e => eventWasRaised = true);


            eventPublisher.Publish(new AnalyticSelectedEvent { AnalyticId = 1, Name = "Analytic Name 1" });
                            
            

            eventWasRaised.Should().BeFalse();

        }

        [TestMethod, TestCategory("Event Aggregrator")]
        public void EventAggregator_SelectedEvent_WithFoundCriteria_SubscriptionIsIgnored()
        {
            var eventWasRaised = false;
            var eventPublisher = new Core.Reactive.EventAggregator();

            eventPublisher.GetEvent<AnalyticSelectedEvent>()
                .Where(e => e.Name == "Analytic Name 1")
                .Subscribe(e => eventWasRaised = true);


            eventPublisher.Publish(new AnalyticSelectedEvent { AnalyticId = 1, Name = "Analytic Name 1" });
            eventWasRaised.Should().BeTrue();

        }

        public List<APLPromoter.Client.Entity.Filter.Value> CreateSampleData()
        {
            return new List<Filter.Value>{
                new Filter.Value{
                    Id = 1,
                    Code = "Code1",
                    Included = true,
                    Name = "Name1"
                },
                new Filter.Value{
                    Id = 2,
                    Code = "Code2",
                    Included = true,
                    Name = "Name2"
                },
                new Filter.Value{
                    Id = 3,
                    Code = "Code3",
                    Included = true,
                    Name = "Name3"
                },
                new Filter.Value{
                    Id = 4,
                    Code = "Code4",
                    Included = true,
                    Name = "Name4"
                },
                new Filter.Value{
                    Id = 5,
                    Code = "Code5",
                    Included = true,
                    Name = "Name5"
                },
                new Filter.Value{
                    Id = 6,
                    Code = "Code6",
                    Included = true,
                    Name = "Name6"
                },
            };
        }
        [TestMethod, TestCategory("Analytic.ViewModel.Identity")]
        public void GivenAnalyticWizardStarted_WhenFiltersAreSelected_DashboardDisplaysResultCounts()
        {

        }
        #endregion



    }
}
