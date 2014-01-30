using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using APLPromoter.Server.Entity;
using APLPromoter.Server.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace APLPromoter.Tests.Server.Data {


    [TestClass]
    public class Analytics {

        private AnalyticData _AnalyticData;
        private System.Diagnostics.TraceListener listener;
        private static String lineBreak = new String('.', 200);
        private const String traceFile = @"\APLPromoter.Tests.Server.Data.log";
        private String SQLKEYINVALID = "00000000-0000-0000-0000-000000000000";
        private String SQLKEYAPLADMIN = "45F2AE12-1428-481E-8A87-43566914B91A"; //APL Administrator
        private String dateStamp = System.DateTime.Now.ToLongDateString();
        private static String debugPath = System.IO.Directory.GetCurrentDirectory();
        private static System.IO.FileStream log = System.IO.File.Open(debugPath + traceFile, FileMode.Append, FileAccess.Write, FileShare.Write);

        [TestInitialize]
        public void Setup() {
            _AnalyticData = new APLPromoter.Server.Data.AnalyticData();
            this.listener = new TextWriterTraceListener(log);
            System.Diagnostics.Trace.Listeners.Add(listener);

            this.listener.WriteLine(String.Format("{0} {1} ms {2} - {3}", this.dateStamp, System.DateTime.Now.ToLongTimeString(), System.DateTime.Now.Millisecond.ToString(), this.GetType().Name)); 
            this.listener.WriteLine(lineBreak);
        }
        [TestCleanup]
        public void Cleanup() {
            this.listener.WriteLine(String.Format("{0} {1} ms {2} - {3}", this.dateStamp, System.DateTime.Now.ToLongTimeString(), System.DateTime.Now.Millisecond.ToString(), this.GetType().Name));
            this.listener.WriteLine(lineBreak); this.listener.WriteLine(lineBreak); this.listener.WriteLine(String.Empty); this.listener.WriteLine(String.Empty); this.listener.WriteLine(String.Empty);
            this.listener.Flush(); 
        }


        //Analytic routine unique identity added to database...
        [TestMethod, TestCategory("Analytics update")]
        public void TEST11_GivenUserInputsAnalyticIdentity_WhenValidAnalyticIdentityAdded_ThenSuccessStatusRecdAndNoValidationMessageRecd() {
            String analyticName = "Add Analytics from Tests.Server.Data"; // + System.DateTime.Now.ToLongTimeString();
            String analyticDescription = "New analytic description, use this to test unique Analytics routine names " + dateStamp;
            var newIdentity = new Analytic.Identity { Name = analyticName, Description = analyticDescription };
            Session<Analytic.Identity> response = _AnalyticData.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEYAPLADMIN, Data = newIdentity });

            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try {
                this.listener.WriteLine(String.Format("Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}", ex.Message)); this.listener.WriteLine(lineBreak);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            Assert.IsTrue(response.SessionOk);
        }

        //Analytic routine identity duplicate name fails validation...
        [TestMethod, TestCategory("Analytics update")]
        public void TEST12_GivenUserInputsAnalyticIdentity_WhenAnalyticIdentityIsDuplicateName_ThenFailedStatusRecdAndValidationMessageRecd() {
            String analyticName = "Analytics from Tests.Server.Data";
            String analyticDescription = "New analytic description, use this to test duplicate Analytics routine names";
            var newIdentity = new Analytic.Identity { Name = analyticName, Description = analyticDescription };
            Session<Analytic.Identity> response = _AnalyticData.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEYAPLADMIN, Data = newIdentity });

            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try {
                this.listener.WriteLine(String.Format("Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);

                Assert.IsFalse(response.SessionOk);
                Assert.AreNotEqual(response.ClientMessage, String.Empty);
                Assert.AreEqual(response.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}", ex.Message)); this.listener.WriteLine(lineBreak);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            Assert.IsFalse(response.SessionOk);
        }

        //Analytic routine identity name > 100 characters fails validation...
        [TestMethod, TestCategory("Analytics update")]
        public void TEST13_GivenUserInputsAnalyticIdentity_WhenAnalyticIdentityNameGreaterThan100Char_ThenFailedStatusRecdAndValidationMessageRecd() {
            String dateStamp = String.Format("{0} {1}", System.DateTime.Now.ToLongDateString(), System.DateTime.Now.ToLongTimeString());
            String analyticName = "Analytics from Tests.Server.Data, this name is too long and will fail validation " + dateStamp;
            String analyticDescription = "New analytic description, use this to test duplicate Analytics routine names " + dateStamp;
            var newIdentity = new Analytic.Identity { Name = analyticName, Description = analyticDescription };

            Assert.IsTrue(newIdentity.Name.Length > 100);
            Session<Analytic.Identity> response = _AnalyticData.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEYAPLADMIN, Data = newIdentity });

            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try {
                this.listener.WriteLine(String.Format("Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);

                Assert.IsFalse(response.SessionOk);
                Assert.AreNotEqual(response.ClientMessage, String.Empty);
                Assert.AreEqual(response.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}", ex.Message)); this.listener.WriteLine(lineBreak);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak); this.listener.WriteLine(lineBreak);
            Assert.IsFalse(response.SessionOk);
        }

        //Analytic routine identity name < 5 characters fails validation...
        [TestMethod, TestCategory("Analytics update")]
        public void TEST14_GivenUserInputsAnalyticIdentity_WhenAnalyticIdentityNameLessThan5Char_ThenFailedStatusRecdAndValidationMessageRecd() {
            String analyticName = new String('0',4);
            String analyticDescription = "New analytic description, use this to test duplicate Analytics routine names";
            var newIdentity = new Analytic.Identity { Name = analyticName, Description = analyticDescription };

            Assert.IsTrue(newIdentity.Name.Length < 5);
            Session<Analytic.Identity> response = _AnalyticData.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEYAPLADMIN, Data = newIdentity });

            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try {
                this.listener.WriteLine(String.Format("Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);

                Assert.IsFalse(response.SessionOk);
                Assert.AreNotEqual(response.ClientMessage, String.Empty);
                Assert.AreEqual(response.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}", ex.Message)); this.listener.WriteLine(lineBreak);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak); this.listener.WriteLine(lineBreak);
            Assert.IsFalse(response.SessionOk);
        }

        //Analytic routine identity invalid session fails validation...
        [TestMethod, TestCategory("Analytics update")]
        public void TEST15_GivenUserInputsAnalyticIdentity_WhenAnalyticIdentitySessionInvalid_ThenFailedStatusRecdAndValidationMessageRecd() {
            var newIdentity = new Analytic.Identity { Name = "Analytics from Tests.Server.Data", Description = "New analytic description" };
            Session<Analytic.Identity> response = _AnalyticData.SaveIdentity(new Session<Analytic.Identity> { SqlKey = SQLKEYINVALID, Data = newIdentity });

            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try {
                this.listener.WriteLine(String.Format("Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);

                Assert.IsFalse(response.SessionOk);
                Assert.AreNotEqual(response.ClientMessage, String.Empty);
                Assert.AreEqual(response.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}", ex.Message)); this.listener.WriteLine(lineBreak);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak); this.listener.WriteLine(lineBreak);
            Assert.IsFalse(response.SessionOk);
        }

        //Analytic routine identity select list...
        [TestMethod, TestCategory("Analytics select")]
        public void TEST16_GivenUserRequestsAnalyticIdentityList_WhenSessionValid_ThenSuccessStatusRecdAndAnalyticsIdentityListRecd() {
            Session<List<Analytic.Identity>> response = _AnalyticData.LoadList(new Session<NullT> { SqlKey = SQLKEYAPLADMIN });

            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try {
                this.listener.WriteLine(String.Format("Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ClientMessage, String.Empty);
                Assert.AreEqual(response.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}", ex.Message)); this.listener.WriteLine(lineBreak);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak); this.listener.WriteLine(lineBreak);
            Assert.IsTrue(response.SessionOk);
        }

        //Analytic routine filters select list...
        [TestMethod, TestCategory("Analytics select")]
        public void TEST17_GivenUserRequestsAnalyticFilterList_WhenSessionValid_ThenSuccessStatusRecdAndAnalyticsFilterListRecd() {
            var newIdentity = new Analytic.Identity { Id = 1, Name = "New Analytic Routine (1)", Description = "New Analytic Routine (1) description" };
            Session<List<APLPromoter.Server.Entity.Filter>> response = _AnalyticData.LoadFilters(new Session<APLPromoter.Server.Entity.Analytic.Identity> { SqlKey = SQLKEYAPLADMIN, Data = newIdentity });

            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try {
                this.listener.WriteLine(String.Format("Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ClientMessage, String.Empty);
                Assert.AreEqual(response.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}", ex.Message)); this.listener.WriteLine(lineBreak);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak); this.listener.WriteLine(lineBreak);
            Assert.IsTrue(response.SessionOk);
        }

        //Analytic routine types select list...
        [TestMethod, TestCategory("Analytics select")]
        public void TEST18_GivenUserRequestsAnalyticTypeList_WhenSessionValid_ThenSuccessStatusRecdAndAnalyticsTypesListRecd() {
            var newIdentity = new Analytic.Identity { Id = 1, Name = "New Analytic Routine (1)", Description = "New Analytic Routine (1) description" };
            Session<List<APLPromoter.Server.Entity.Analytic.Type>> response = _AnalyticData.LoadTypes(new Session<APLPromoter.Server.Entity.Analytic.Identity> { SqlKey = SQLKEYAPLADMIN, Data = newIdentity });

            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try {
                this.listener.WriteLine(String.Format("Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ClientMessage, String.Empty);
                Assert.AreEqual(response.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}", ex.Message)); this.listener.WriteLine(lineBreak);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak); this.listener.WriteLine(lineBreak);
            Assert.IsTrue(response.SessionOk);
        }

        //Analytic routine save filters with validation...
        [TestMethod, TestCategory("Analytics update")]
        public void TEST19_GivenUserInputsAnalyticFilters_WhenAnalyticFilterSessionValid_ThenSuccessStatusRecdAndValidFiltersSaved() {
            Boolean sessionLoaded = false;
            var newIdentity = new Analytic.Identity { Id = 1, Name = "New Analytic Routine (1)", Description = "New Analytic Routine (1) description" };
            Session<List<APLPromoter.Server.Entity.Filter>> responseLoad = _AnalyticData.LoadFilters(new Session<APLPromoter.Server.Entity.Analytic.Identity> { SqlKey = SQLKEYAPLADMIN, Data = newIdentity });

            this.listener.WriteLine("Begin filter load - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try {
                this.listener.WriteLine(String.Format("Session valid: {0}", (responseLoad.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Client message: {0}", responseLoad.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Server message: {0}", responseLoad.ServerMessage)); this.listener.WriteLine(lineBreak);
                sessionLoaded = responseLoad.SessionOk;
                Assert.IsTrue(responseLoad.SessionOk);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}", ex.Message)); this.listener.WriteLine(lineBreak);
            }

            if (sessionLoaded) {
                foreach (APLPromoter.Server.Entity.Filter filter in responseLoad.Data) {
                    foreach (APLPromoter.Server.Entity.Filter.Value value in filter.Values) {
                        if (value.Key.CompareTo(1073) == 0) { value.Included = (value.Included) ? false : true; }
                        if (value.Key.CompareTo(1093) == 0) { value.Included = (value.Included) ? false : true; }
                    }
                }
                var newAnalytic = new APLPromoter.Server.Entity.Analytic { Self = newIdentity, Filters = responseLoad.Data };
                Session<List<APLPromoter.Server.Entity.Filter>> responseSave = _AnalyticData.SaveFilters(new Session<Analytic> { SqlKey = SQLKEYAPLADMIN, Data = newAnalytic });

                this.listener.WriteLine("Begin filter save - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
                try {
                    this.listener.WriteLine(String.Format("Session valid: {0}", (responseSave.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                    this.listener.WriteLine(String.Format("Client message: {0}", responseSave.ClientMessage)); this.listener.WriteLine(lineBreak);
                    this.listener.WriteLine(String.Format("Server message: {0}", responseSave.ServerMessage)); this.listener.WriteLine(lineBreak);

                    Assert.IsTrue(responseSave.SessionOk);
                    Assert.AreEqual(responseSave.ClientMessage, String.Empty);
                    Assert.AreEqual(responseSave.ServerMessage, String.Empty);
                }
                catch (System.Exception ex) {
                    this.listener.WriteLine(String.Format("Exception: {0}", ex.Message)); this.listener.WriteLine(lineBreak);
                    this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
                }
                Assert.IsTrue(responseSave.SessionOk);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            Assert.IsTrue(responseLoad.SessionOk);
        }

        //Analytic routine save types with validation...
        [TestMethod, TestCategory("Analytics update")]
        public void TEST20_GivenUserInputsAnalyticTypes_WhenAnalyticTypesSessionValid_ThenSuccessStatusRecdAndValidTypesSaved() {
            Boolean sessionLoaded = false;
            var newIdentity = new Analytic.Identity { Id = 1, Name = "New Analytic Routine (1)", Description = "New Analytic Routine (1) description" };
            Session<List<APLPromoter.Server.Entity.Analytic.Type>> responseLoad = _AnalyticData.LoadTypes(new Session<APLPromoter.Server.Entity.Analytic.Identity> { SqlKey = SQLKEYAPLADMIN, Data = newIdentity });

            this.listener.WriteLine("Begin type load - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try {
                this.listener.WriteLine(String.Format("Session valid: {0}", (responseLoad.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Client message: {0}", responseLoad.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Server message: {0}", responseLoad.ServerMessage)); this.listener.WriteLine(lineBreak);
                sessionLoaded = responseLoad.SessionOk;
                Assert.IsTrue(responseLoad.SessionOk);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}", ex.Message)); this.listener.WriteLine(lineBreak);
            }

            if (sessionLoaded) {
                var newAnalytic = new APLPromoter.Server.Entity.Analytic { Self = newIdentity, Types = responseLoad.Data };
                Session<List<APLPromoter.Server.Entity.Analytic.Type>> responseSave = _AnalyticData.SaveTypes(new Session<Analytic> { SqlKey = SQLKEYAPLADMIN, Data = newAnalytic });

                this.listener.WriteLine("Begin type save - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
                try {
                    this.listener.WriteLine(String.Format("Session valid: {0}", (responseSave.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                    this.listener.WriteLine(String.Format("Client message: {0}", responseSave.ClientMessage)); this.listener.WriteLine(lineBreak);
                    this.listener.WriteLine(String.Format("Server message: {0}", responseSave.ServerMessage)); this.listener.WriteLine(lineBreak);

                    Assert.IsTrue(responseSave.SessionOk);
                    Assert.AreEqual(responseSave.ClientMessage, String.Empty);
                    Assert.AreEqual(responseSave.ServerMessage, String.Empty);
                }
                catch (System.Exception ex) {
                    this.listener.WriteLine(String.Format("Exception: {0}", ex.Message)); this.listener.WriteLine(lineBreak);
                    this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
                }
                Assert.IsTrue(responseSave.SessionOk);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            Assert.IsTrue(responseLoad.SessionOk);
        }

    }
}
