
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using APLPromoter.Server;
using APLPromoter.Server.Data;
using APLPromoter.Server.Entity;
using APLPromoter.Server.Services.Contracts;
using FluentAssertions;
using APLPromoter.Server.Services;
using System.Diagnostics;


namespace APLPromoter.Tests.Server.Services
{
    [TestClass]
    public class Analytics
    {
        private IAnalyticService _AnalyticService;
        private IAnalyticData _MockAnalyticData;
        private User _User = null;
        private const string SQLKEY = "1";
        private const string SQLKEY_DEFAULT = "2";
        private const string SQLKEY_PRICINGANALYST = "3";
        private const string SQLKEY_ADMIN = "45F2AE12-1428-481E-8A87-43566914B91A";
        private string DATE = string.Format("{0} {1}", DateTime.Now.ToLongDateString(),DateTime.Now.ToLongTimeString());

        [TestInitialize]
        public void Setup ()
        {
            //_MockAnalyticData = A.Fake<IAnalyticData>();
            //_MockAnalyticData = new MockAnalyticData();
            _MockAnalyticData = new AnalyticData();
            _AnalyticService = new AnalyticService(_MockAnalyticData);

        }
   
        
        //Analytic routine name is between 5 and 50 characters
        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenUserInputsRoutineNameBetween5and50Chars_WhenAnalyticAdded_ThenSuccessStatusRecdAndNoValidationErrorRecd()
        {
            var validAnalytic = new Analytic.Identity { Name = "my analytic" + DATE , Description = "a valid description" };
            Session<Analytic.Identity> response = _AnalyticService.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEY_ADMIN, Data = validAnalytic });
            response.SessionOk.Should().BeTrue();
            response.ServerMessage.Should().BeEmpty();
            response.ClientMessage.Should().BeEmpty();

            var dupAnalytic = new Analytic.Identity { Name = "my analytic" + DATE, Description = "a valid description" };
            Session<Analytic.Identity> responseDup = _AnalyticService.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEY_ADMIN, Data = dupAnalytic });
            responseDup.SessionOk.Should().BeFalse();
            responseDup.ClientMessage.Should().Contain("Duplicate");
            responseDup.ServerMessage.Should().BeEmpty();

        }

        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenUserInputsAnalyticNameWith2Chars_WhenAnalyticAdded_ThenValidationErrorRecd()
        {
            var invalidAnalytic = new Analytic.Identity { Name = "12", Description = "a valid description" };
            var response = _AnalyticService.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEY_ADMIN, Data = invalidAnalytic });
            response.SessionOk.Should().BeFalse();
            response.ClientMessage.Should().Contain("Valid Analytics routine names are between 5 and 100 characters");


        }

        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenUserInputsAnalyticNameWith51Chars_WhenAnalyticAdded_ThenValidationErrorRecd()
        {   string nameLong = new string('a', 101);
            var invalidAnalytic = new Analytic.Identity { Name = nameLong, Description = "valid description" };
            var response = _AnalyticService.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEY_ADMIN, Data = invalidAnalytic });
            response.SessionOk.Should().BeFalse();
            response.ClientMessage.Should().NotBeBlank();

        }

        //Analytic description is between 0 and 250 characters
        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenUserInputsAnalyticDescriptionBetween5and150Chars_WhenAnalyticAdded_ThenSuccessStatusRecdAndNoValidationErrorRecd()
        {
            //Arrange
            var invalidAnalytic = new Analytic.Identity { Name = "Valid Name" + DATE, Description = "A valid description" };
            //Act
            var response = _AnalyticService.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEY_ADMIN, Data = invalidAnalytic });
            //Assert
            response.SessionOk.Should().BeTrue();
            response.ClientMessage.Should().BeBlank();
            response.ServerMessage.Should().BeBlank();
            

        }

        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenUserInputsAnalyticDescriptionWith2Chars_WhenAnalyticAdded_ThenValidationErrorRecd()
        {
            //Arrange
            var analytic = new Analytic.Identity{
                Name = "valid description",
                Description = "22"
            };
            //Act
            var response = _AnalyticService.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEY_ADMIN, Data = analytic });
            //Assert
            response.SessionOk.Should().BeFalse();
            response.ClientMessage.Should().NotBeBlank();

        }

        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenUserInputsRoutineDescriptionWith151Chars_WhenAnalyticAdded_ThenValidationErrorRecd()
        {
            //Arrange
            var analytic = new Analytic.Identity
            {
                Name = "valid name",
                Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis pa"
            };
            //Act
            var response = _AnalyticService.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEY_ADMIN, Data = analytic });
            //Assert
            response.SessionOk.Should().BeFalse();
            response.ClientMessage.Should().NotBeBlank();

        }

        //Administrators can create Analytic routines
        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenUserIsAdmin_WhenAnalyticAdded_ThenSuccessStatusRecd()
        {
            //Arrange
            var analytic = new Analytic.Identity
            {
                Name = "valid name" + DATE,
                Description = "valid description "
            };
            //Act
            var response = _AnalyticService.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEY_ADMIN, Data = analytic });
            //Assert
            response.SessionOk.Should().BeTrue();
            response.ClientMessage.Should().BeBlank();           

        }

        //Pricing Analysts can create Analytic routines
        [TestMethod, TestCategory("Analytic.Services"), Ignore]
        public void GivenUserIsPricingAnalyst_WhenAnalyticAdded_ThenSuccessStatusRecd()
        {
            //Arrange
            var analytic = new Analytic.Identity
            {
                Name = "valid name",
                Description = "valid description"
            };
            //Act
            var response = _AnalyticService.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEY_PRICINGANALYST, Data = analytic });
            //Assert
            response.SessionOk.Should().BeTrue();
            response.ClientMessage.Should().BeBlank();    

        }

        //Pricing Analysts can create Analytic routines
        [TestMethod, TestCategory("Analytic.Services"),Ignore]
        public void GivenUserIsNonAdmin_WhenAnalyticAdded_ThenFailureStatusRecd()
        {
            //Arrange
            var analytic = new Analytic.Identity
            {
                Name = "valid name" + DATE,
                Description = "valid description"
            };
            //Act
            var response = _AnalyticService.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEY_DEFAULT, Data = analytic });
            //Assert
            response.SessionOk.Should().BeTrue();
            response.ClientMessage.Should().NotBeBlank();  

        }

        //If the user does not provide an Analytic descriptions the name becomes the default
        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenUserProvidesNoDescription_WhenAnalyticAdded_ThenDescriptionIsDefaultedToName()
        {
            //Arrange
            var expectedAnalytic = new Analytic.Identity
            {
                Name = "valid name" + DATE,
                Description = ""
            };
            //Act
            var response = _AnalyticService.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEY_ADMIN, Data = expectedAnalytic });
            //Assert
            response.ClientMessage.Should().BeBlank();  
            response.Data.Description.ShouldBeEquivalentTo(expectedAnalytic.Name);

        }

        //Analytic routines are tracked by author, editors, owners
        [TestMethod, TestCategory("Analytic.Services"), Ignore]
        public void GivenAnyAnalytic_WhenAdded_ThenAuthorEditorOwnerIsAudited()
        {
            //Arrange
            var analytic = new Analytic.Identity
            {
                Name = "valid name" + DATE,
                Description = "valid analytic"
            };
            //Act
            var response = _AnalyticService.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEY_ADMIN, Data = analytic });
            //Assert
            response.ClientMessage.Should().NotBeBlank();  
        }
        
        // Administrators can set whether other users can create Analytic routines
        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenUserIsAdministrator_WhenAdminEnablesCreateAnalyticRoleForAnyUser_UserCanCreateAnalytics(){}

        // All filter types have at least one filter value included
        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenAnalyticWithNoFilters_WhenFilterSaved_ThenFilterErrorRequiredValidationMessageRecd()
        {
            //Arrange
            var analytic = new Analytic
            {
                Self = new Analytic.Identity
                {
                    Name = "valid name" + DATE,
                    Description = "valid analytic"
                }
            };

            //Act
            var response = _AnalyticService.SaveFilters(new Session<Analytic>() { SqlKey = SQLKEY_ADMIN, Data = analytic });

            //Assert
            response.ServerMessage.Should().NotBeBlank();
            System.Diagnostics.Trace.WriteLine("---------->" + response.ServerMessage);
            
        }

        // User cannot save analysis without defining Movement group parameters
        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenAnalyticWithNoMovementTypeDefined_WhenAnalyticTypeSaved_ThenErroMovementRequiredMessageRecd() { }

        /* Auto generate groups…
                - Number of groups between 1 and 15
                - If min outlier > 0 then max outlier > min */
        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenAnalyticWithAutoGroups_Given16Groups_WhenAnalyticTypeSaved_ErrorMessageRecd() {
            //Arrange
            //Save ID 
            var validAnalytic = new Analytic.Identity { Name = "my analytic" + DATE, Description = "a valid description" };
            Session<Analytic.Identity> response = _AnalyticService.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEY_ADMIN, Data = validAnalytic });
            response.SessionOk.Should().BeTrue();
            response.ServerMessage.Should().BeEmpty();
            response.ClientMessage.Should().BeEmpty();

            //LoadTypes
            Session<List<Analytic.Driver>> responseLoadTypes = 
                _AnalyticService.LoadTypes(new Session<Analytic.Identity>{SqlKey = SQLKEY_ADMIN, Data = validAnalytic});
            var selectedType = responseLoadTypes.Data.FirstOrDefault();
            var list = new List<Analytic.Driver>();
            //Modify
            selectedType.Selected = true;
            selectedType.Modes[0].Selected = true;
            selectedType.Modes[0].Groups[0].Value = 2;
            selectedType.Modes[0].Groups[0].MinOutlier = 20;
            selectedType.Modes[0].Groups[0].MaxOutlier = 40;

            list.Add(selectedType);
            var analytic = new Analytic { Self = validAnalytic, Drivers = list };                       
                                    



            //var analyticTypes = new Analytic
            //{
            //    Filters = new List<Filter>{
            //                        new Filter{},
            //                        new Filter{}
            //    },
            //    Types = new List<Analytic.Type>{
            //        new Analytic.Type{
            //            Id = response.Data.Id,
            //            Key = (Int32) AnalyticTypes.Movement,
            //            Selected = true,
            //            Modes = new List<Analytic.Type.Mode>{
            //                new Analytic.Type.Mode{
            //                    Key = (Int32) ModeType.Manual,
            //                    Selected = true,
            //                    Groups = new List<Analytic.Type.Mode.Group>{
            //                        //new Analytic.Type.Mode.Group{
            //                        //    Id = 1,
            //                        //    MinOutlier = 6,
            //                        //    MaxOutlier = 8,
            //                        //    Value = 2
            //                        }
            //                        //,
            //                        //new Analytic.Type.Mode.Group{
            //                        //    Id = 2,
            //                        //    MinOutlier = 8,
            //                        //    MaxOutlier = 6,
            //                        //    Value = 2
            //                        //},
            //                        //new Analytic.Type.Mode.Group{
            //                        //    Id = 3,
            //                        //    MinOutlier = 8,
            //                        //    MaxOutlier = 6,
            //                        //    Value = 2
            //                        //},
            //                        //new Analytic.Type.Mode.Group{
            //                        //    Id = 4,
            //                        //    MinOutlier = 8,
            //                        //    MaxOutlier = 6,
            //                        //    Value = 2
            //                        //}

            //                    }
            //                },

            //            }
            //        }
            //    };
                //,
                //Self = new Analytic.Identity
                //{
                //    Name = "valid name" + DATE,
                //    Description = "valid analytic"
                //}
 
            //Act
            var responseSaveTypes = _AnalyticService.SaveTypes(new Session<Analytic> { SqlKey = SQLKEY_ADMIN, Data = analytic });
            //Assert
            responseSaveTypes.SessionOk.Should().BeTrue();
            responseSaveTypes.ClientMessage.Should().BeBlank();             
            Debug.WriteLine(response.ClientMessage);
        }


        //APLPromoterServerData: dbo.aplAnalyticsUpdate, updateTypes, System.Data, DataTableReader is invalid for current DataTable 'reader'. 

        /* Manual generated groups…
                -          Number of groups between 1 and 15
                -          Min outliers must be less than max outliers
                -          Each group min outlier must be less than next group min outlier
                -          Each group max outlier must be less than next group max outlier */
        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenValidGroups_GivenValidOutliers_WhenTypesSaved_SuccessSavingTypes()
        {
            //Arrange
            var analytic = new Analytic
            {
                Filters = new List<Filter>{
                                    new Filter{},
                                    new Filter{}
                },
                Drivers = new List<Analytic.Driver>{
                    new Analytic.Driver {
                        Modes = new List<Analytic.Driver.Mode>{
                            new Analytic.Driver.Mode{
                                Key = (int) ModeType.Manual,
                                Groups = new List<Analytic.Driver.Mode.Group>{
                                    new Analytic.Driver.Mode.Group{
                                        Id = 1,
                                        MinOutlier = 8,
                                        MaxOutlier = 6,
                                        Value = 2
                                    }
                                }
                            },

                        }
                    }
                },
                Self = new Analytic.Identity
                {
                    Name = "valid name" + DATE,
                    Description = "valid analytic"
                }
            };
            //Act
            var response = _AnalyticService.SaveTypes(new Session<Analytic> { SqlKey = SQLKEY_ADMIN, Data = analytic });
            //Assert
            response.SessionOk.Should().BeFalse();
            response.ClientMessage.Should().NotBeBlank();
            Debug.WriteLine(response.ClientMessage);

        }


        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenAnalyticWithManualGroups_GivenValidNumGroups_GivenMinOutliersGreaterThanMaxOutliers_WhenTypesSaved_ErrorMessageRecd() {

            //Arrange
            var analytic = new Analytic
            {
                Filters = new List<Filter>{
                                    new Filter{},
                                    new Filter{}
                },
                Drivers = new List<Analytic.Driver>{
                    new Analytic.Driver {
                        Modes= new List<Analytic.Driver.Mode>{
                            new Analytic.Driver.Mode{
                                Key = (int) ModeType.Manual,
                                Groups = new List<Analytic.Driver.Mode.Group>{
                                    new Analytic.Driver.Mode.Group{
                                        Id = 1,
                                        MinOutlier = 8,
                                        MaxOutlier = 6,
                                        Value = 2
                                    }
                                }
                            },

                        }
                    }
                },
                Self = new Analytic.Identity
                {
                    Name = "valid name",
                    Description = "valid analytic"
                }
            };
            //Act
            var response = _AnalyticService.SaveTypes(new Session<Analytic> { SqlKey = SQLKEY_ADMIN, Data = analytic });
            //Assert
            response.SessionOk.Should().BeFalse();
            response.ClientMessage.Should().NotBeBlank();
            Debug.WriteLine(response.ClientMessage);
        }

        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenAnalyticWithManualGroups_GivenZeroGroups_WhenTypesSaved_ErrorMessageRecd()
        {

            //Arrange
            var analytic = new Analytic
            {
                Filters = new List<Filter>{
                                    new Filter{},
                                    new Filter{}
                },
                Drivers = new List<Analytic.Driver>{
                    new Analytic.Driver {
                        Modes= new List<Analytic.Driver.Mode>{
                            new Analytic.Driver.Mode{
                                Key = (int) ModeType.Manual,
                                Groups = new List<Analytic.Driver.Mode.Group>{
                                    new Analytic.Driver.Mode.Group{
                                        Id = 1,
                                        MinOutlier = 8,
                                        MaxOutlier = 6,
                                        Value = 2
                                    }
                                }
                            },

                        }
                    }
                },
                Self = new Analytic.Identity
                {
                    Name = "valid name",
                    Description = "valid analytic"
                }
            };
            //Act
            var response = _AnalyticService.SaveTypes(new Session<Analytic> { SqlKey = SQLKEY_ADMIN, Data = analytic });
            //Assert
            response.ServerMessage.Should().Contain("Analytic.Types.Save - Invalid Groups");
        }


        // User cannot save analysis without defining group parameters for additional selected analysis types
        //outside of movement - eg. days on hand is included and has no groups
        [TestMethod, TestCategory("Analytic.Services")]
        public void GivenAnalyticWithAnyNoOfAnalyticTypes_Given0GroupParameters_WhenTypesSaved_ErrorMessageRecd() {

            //Arrange
            var analytic = new Analytic
            {
                Filters = new List<Filter>{
                                    new Filter{},
                                    new Filter{}
                },
                Drivers = new List<Analytic.Driver>{
                    new Analytic.Driver {
                        Modes= new List<Analytic.Driver.Mode>{
                            new Analytic.Driver.Mode{
                                Key = (int) ModeType.Manual,
                                Groups = new List<Analytic.Driver.Mode.Group>{
                                    new Analytic.Driver.Mode.Group{
                                        Id = 1,
                                        MinOutlier = 8,
                                        MaxOutlier = 6,
                                        Value = 2
                                    }
                                }
                            },

                        }
                    },
                     new Analytic.Driver {
                        Modes = new List<Analytic.Driver.Mode>{
                            new Analytic.Driver.Mode{
                                Key = (int) ModeType.Manual,
                                Groups = new List<Analytic.Driver.Mode.Group>{
                                    new Analytic.Driver.Mode.Group{
                                        Id = 2,
                                        MinOutlier = 3,
                                        MaxOutlier = 4,
                                        Value = 5
                                    }
                                }
                            },

                        }
                     } 
                },

                Self = new Analytic.Identity
                {
                    Id = 1, //to get types and filters
                    Name = "valid name",
                    Description = "valid analytic"
                }
            };
            //Act
            var response = _AnalyticService.SaveTypes(new Session<Analytic> { SqlKey = SQLKEY_ADMIN, Data = analytic });
            //Assert
            response.ServerMessage.Should().Contain("Analytic.Types.Save - Invalid Groups");
        
        }

        [TestMethod, Ignore]
        public void GivenValidAnalyticCreated_GivenLinkedToPriceRoutine_WhenAnalyticDeleted_ThenExpectations()
        {
            //Arrange

            //Act - AnalyticService.Delete

            //Assert
        }


    }
}
