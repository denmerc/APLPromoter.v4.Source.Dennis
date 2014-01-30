using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using APLPromoter.Server.Entity;
using APLPromoter.Server.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace APLPromoter.Tests.Server.Data {

    [TestClass]
    public class User {

        private UserData _UserData;
        private System.Diagnostics.TraceListener listener;
        private static String lineBreak = new String('.', 200);
        private const String traceFile = @"\APLPromoter.Tests.Server.Data.log";
        private String SQLKEYSHARED = "72B9ED08-5D12-48FD-9CF7-56A3CA30E660"; //Shared application key
        private String SQLKEYPRIVATE = "9C8B31D8-ACD5-446A-912E-3019BAF05E6C"; //Private customer key
        private String dateStamp = System.DateTime.Now.ToLongDateString();
        private static String debugPath = System.IO.Directory.GetCurrentDirectory();
        private static System.IO.FileStream log = System.IO.File.Open(debugPath + traceFile, FileMode.Append, FileAccess.Write, FileShare.Write);

        [TestInitialize]
        public void Setup() {
            _UserData = new APLPromoter.Server.Data.UserData();
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

        //Application initialization (test-pass)...
        [TestMethod, TestCategory("Application pass")]
        public void TEST01_GivenUserExecutesApplication_WhenRequestReceived_ThenSuccessStatusRecdAndSQLPrivateKeyRecd() {
            Session<NullT> response = _UserData.Initialize(new Session<NullT> { SqlKey = SQLKEYSHARED });

            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try {
                this.listener.WriteLine(String.Format("Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("SQL Private key: {0}", response.SqlKey)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ServerMessage, String.Empty);
                Assert.AreEqual(response.SqlKey, this.SQLKEYPRIVATE);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}", ex.Message)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            Assert.IsTrue(response.SessionOk);
        }

        //SQL Server Authentication (test-pass)...
        [TestMethod, TestCategory("Authentication pass")]
        public void TEST02_GivenUserRequestsAuthentication_WhenApplicationInitialized_ThenSuccessStatusRecdAndSQLSessionKeyRecd() {
            Session<NullT> response = null;

            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try {
                response = _UserData.Initialize(new Session<NullT> { SqlKey = SQLKEYSHARED });
                this.listener.WriteLine(String.Format("Init Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init SQL Private key: {0}", response.SqlKey)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ServerMessage, String.Empty);
                Assert.AreEqual(response.SqlKey, this.SQLKEYPRIVATE);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }

            try {
                response.UserIdentity = new APLPromoter.Server.Entity.User.Identity() ;
                response.UserIdentity.Login = "Administrator";
                response.UserIdentity.Password = new APLPromoter.Server.Entity.User.Password { Old = "password" };
                response = _UserData.Authenticate(response);
                this.listener.WriteLine(String.Format("Authenticate Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate User Authenticated: {0}", (response.Authenticated) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate SQL Session key: {0}", response.SqlKey)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Explorer Planning: {0}", response.UserIdentity.Role.Planning)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ServerMessage, String.Empty);
                Assert.IsTrue(response.UserIdentity.Role.Planning.Contains(response.SqlKey.ToUpper()));
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            Assert.IsTrue(response.SessionOk);
        }

        //Administrator Select Identities (test-pass)...
        [TestMethod, TestCategory("Administration select data pass")]
        public void TEST03_GivenRequestUserIdentityList_WhenAdminAuthenticated_ThenSuccessStatusRecdAndUserIdentityListRecd() {
            Session<NullT> response = null;
            Session<List<APLPromoter.Server.Entity.User.Identity>> list = null;

            #region Initialize...
            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try { //Initialize...
                response = _UserData.Initialize(new Session<NullT> { SqlKey = SQLKEYSHARED });
                this.listener.WriteLine(String.Format("Init Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init SQL Private key: {0}", response.SqlKey)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ServerMessage, String.Empty);
                Assert.AreEqual(response.SqlKey, this.SQLKEYPRIVATE);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            #endregion

            #region Authenticate...
            try { //Authentiate...
                response.UserIdentity = new APLPromoter.Server.Entity.User.Identity();
                response.UserIdentity.Login = "Administrator";
                response.UserIdentity.Password = new APLPromoter.Server.Entity.User.Password { Old = "password" };
                response = _UserData.Authenticate(response);
                this.listener.WriteLine(String.Format("Authenticate Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate User Authenticated: {0}", (response.Authenticated) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate SQL Session key: {0}", response.SqlKey)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Explorer Planning: {0}", response.UserIdentity.Role.Planning)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ServerMessage, String.Empty);
                Assert.IsTrue(response.UserIdentity.Role.Planning.Contains(response.SqlKey.ToUpper()));
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            #endregion 

            try { //TEST Administrator Select Identities...
                list = _UserData.LoadList(response);
                this.listener.WriteLine(String.Format("Authenticate Session valid: {0}", (list.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Client message: {0}", list.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Server message: {0}", list.ServerMessage)); this.listener.WriteLine(lineBreak);
                foreach (APLPromoter.Server.Entity.User.Identity identity in list.Data)
                    this.listener.WriteLine(String.Format("User: {0}, {1}, {2}, {3}", identity.Name,identity.Login,identity.Role.Name,identity.EditedText)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(list.SessionOk);
                Assert.AreEqual(list.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            Assert.IsTrue(response.SessionOk);
            Assert.IsTrue(list.SessionOk);
        }

        //Administrator Select Identity from list (test-pass)...
        [TestMethod, TestCategory("Administration select data pass")]
        public void TEST04_GivenRequestUserIdentity_WhenAdminAuthenticatedAndValidIdentitiesList_ThenSuccessStatusRecdAndUserIdentityRecd() {
            Int32 thisUser = 0;
            Session<NullT> response = null;
            Session<APLPromoter.Server.Entity.User.Identity> user = null;
            Session<List<APLPromoter.Server.Entity.User.Identity>> list = null;

            #region Initialize...
            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try { //Initialize...
                response = _UserData.Initialize(new Session<NullT> { SqlKey = SQLKEYSHARED });
                this.listener.WriteLine(String.Format("Init Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init SQL Private key: {0}", response.SqlKey)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ServerMessage, String.Empty);
                Assert.AreEqual(response.SqlKey, this.SQLKEYPRIVATE);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            #endregion

            #region Authenticate...
            try { //Authentiate...
                response.UserIdentity = new APLPromoter.Server.Entity.User.Identity();
                response.UserIdentity.Login = "Administrator";
                response.UserIdentity.Password = new APLPromoter.Server.Entity.User.Password { Old = "password" };
                response = _UserData.Authenticate(response);
                this.listener.WriteLine(String.Format("Authenticate Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate User Authenticated: {0}", (response.Authenticated) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate SQL Session key: {0}", response.SqlKey)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Explorer Planning: {0}", response.UserIdentity.Role.Planning)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ServerMessage, String.Empty);
                Assert.IsTrue(response.UserIdentity.Role.Planning.Contains(response.SqlKey.ToUpper()));
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            #endregion

            #region Load Identities...
            try { //Administrator Select Identities...
                list = _UserData.LoadList(response);
                this.listener.WriteLine(String.Format("Authenticate Session valid: {0}", (list.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Client message: {0}", list.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Server message: {0}", list.ServerMessage)); this.listener.WriteLine(lineBreak);
                foreach (APLPromoter.Server.Entity.User.Identity identity in list.Data)
                    this.listener.WriteLine(String.Format("User: {0}, {1}, {2}, {3}", identity.Name, identity.Login, identity.Role.Name, identity.EditedText)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(list.SessionOk);
                Assert.AreEqual(list.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            #endregion

            try { //TEST - Administrator Select Identity...
                user = _UserData.LoadIdentity(new Session<APLPromoter.Server.Entity.User.Identity> {
                    SqlKey = list.SqlKey,
                    UserIdentity = list.UserIdentity,
                    AppOnline = list.AppOnline,
                    Authenticated = list.Authenticated,
                    SqlAuthorization = list.SqlAuthorization,
                    WinAuthorization = list.WinAuthorization,
                    Data = new APLPromoter.Server.Entity.User.Identity { Id = list.Data[thisUser].Id } 
                });

                this.listener.WriteLine(String.Format("Authenticate Session valid: {0}", (list.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Client message: {0}", list.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Server message: {0}", list.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Selected User: {0}, {1}, {2}, {3}", user.Data.Name, user.Data.Login, user.Data.Role.Name, user.Data.EditedText)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(list.SessionOk);
                Assert.AreEqual(list.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            Assert.IsTrue(response.SessionOk);
            Assert.IsTrue(list.SessionOk);
            Assert.IsTrue(user.SessionOk);
        }

        //Change user password (test-pass)...
        [TestMethod, TestCategory("Authentication update data pass")]
        public void TEST05_GivenUserRequestsChangePassword_WhenUserAuthenticated_ThenSuccessStatusRecdAndPasswordChangeRecd() {
            Session<NullT> response = null;

            #region Initialize...
            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try { //Initialize application...
                response = _UserData.Initialize(new Session<NullT> { SqlKey = SQLKEYSHARED });
                this.listener.WriteLine(String.Format("Init Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init SQL Private key: {0}", response.SqlKey)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ServerMessage, String.Empty);
                Assert.AreEqual(response.SqlKey, this.SQLKEYPRIVATE);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
            }
            #endregion

            #region Authenticate...
            try { //Authenticate user
                response.UserIdentity = new APLPromoter.Server.Entity.User.Identity();
                response.UserIdentity.Login = "Administrator";
                response.UserIdentity.Password = new APLPromoter.Server.Entity.User.Password { Old = "password" };
                response = _UserData.Authenticate(response);
                this.listener.WriteLine(String.Format("Authenticate Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate User Authenticated: {0}", (response.Authenticated) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate SQL Session key: {0}", response.SqlKey)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Explorer Planning: {0}", response.UserIdentity.Role.Planning)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ServerMessage, String.Empty);
                Assert.IsTrue(response.UserIdentity.Role.Planning.Contains(response.SqlKey.ToUpper()));
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
            }
            #endregion

            try { //TEST - Change user password...
                response.UserIdentity.Password = new APLPromoter.Server.Entity.User.Password { Old = "password", New = "NEWpassword" };
                response = _UserData.SavePassword(response);
                this.listener.WriteLine(String.Format("Authenticate Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate User Authenticated: {0}", (response.Authenticated) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate SQL Session key: {0}", response.SqlKey)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Change: Old password: {0}", response.UserIdentity.Password.Old)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Change: New password: {0}", response.UserIdentity.Password.New)); this.listener.WriteLine(lineBreak);

                //Verify password changed..
                response.UserIdentity.Password.Old = "NEWpassword";
                response.UserIdentity.Password.New = "password";
                response = _UserData.SavePassword(response);
                this.listener.WriteLine(String.Format("Verify: Old password: {0}", response.UserIdentity.Password.Old)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Verify: New password: {0}", response.UserIdentity.Password.New)); this.listener.WriteLine(lineBreak);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            Assert.IsTrue(response.SessionOk);
        }

        //Administrator Edit User Identity (test-pass)...
        [TestMethod, TestCategory("Administration update data pass")]
        public void TEST06_GivenRequestSaveUserIdentity_WhenAdminAuthenticatedAndValidIdentityEdited_ThenSuccessStatusRecdAndUserIdentityRecd() {
            Int32 selectedUser = 0;
            Session<NullT> response = null;
            Session<APLPromoter.Server.Entity.User.Identity> user = null;
            Session<List<APLPromoter.Server.Entity.User.Identity>> list = null;

            #region Initialize...
            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try { //Initialize...
                response = _UserData.Initialize(new Session<NullT> { SqlKey = SQLKEYSHARED });
                this.listener.WriteLine(String.Format("Init Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init SQL Private key: {0}", response.SqlKey)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ServerMessage, String.Empty);
                Assert.AreEqual(response.SqlKey, this.SQLKEYPRIVATE);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            #endregion

            #region Authenticate...
            try { //Authentiate...
                response.UserIdentity = new APLPromoter.Server.Entity.User.Identity();
                response.UserIdentity.Login = "Administrator";
                response.UserIdentity.Password = new APLPromoter.Server.Entity.User.Password { Old = "password" };
                response = _UserData.Authenticate(response);
                this.listener.WriteLine(String.Format("Authenticate Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate User Authenticated: {0}", (response.Authenticated) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate SQL Session key: {0}", response.SqlKey)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Explorer Planning: {0}", response.UserIdentity.Role.Planning)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ServerMessage, String.Empty);
                Assert.IsTrue(response.UserIdentity.Role.Planning.Contains(response.SqlKey.ToUpper()));
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            #endregion

            #region Load Identities...
            try { //Administrator Select Identities...
                list = _UserData.LoadList(response);
                this.listener.WriteLine(String.Format("Load Session valid: {0}", (list.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Load Client message: {0}", list.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Load Server message: {0}", list.ServerMessage)); this.listener.WriteLine(lineBreak);
                foreach (APLPromoter.Server.Entity.User.Identity identity in list.Data)
                    this.listener.WriteLine(String.Format("User: {0}, {1}, {2}, {3}", identity.Name, identity.Login, identity.Role.Name, identity.EditedText)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(list.SessionOk);
                Assert.AreEqual(list.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            #endregion

            #region Load Identity...
            try { //Administrator Select Identity...
                user = _UserData.LoadIdentity(new Session<APLPromoter.Server.Entity.User.Identity> {
                    SqlKey = list.SqlKey,
                    UserIdentity = list.UserIdentity,
                    AppOnline = list.AppOnline,
                    Authenticated = list.Authenticated,
                    SqlAuthorization = list.SqlAuthorization,
                    WinAuthorization = list.WinAuthorization,
                    Data = new APLPromoter.Server.Entity.User.Identity { Id = list.Data[selectedUser].Id }
                });

                this.listener.WriteLine(String.Format("Select Session valid: {0}", (user.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Select Client message: {0}", user.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Select Server message: {0}", user.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Selected User: {0}, {1}, {2}, {3}", user.Data.Name, user.Data.Login, user.Data.Email, user.Data.EditedText)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(user.SessionOk);
                Assert.AreEqual(user.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            #endregion

            try { //TEST - Administrator Edit Identity...
                String edited = "-Edited";
                String password="password";
                String oldPassword = (user.Data.Login.Contains(edited)) ? password+edited : password;
                String newPassword = (user.Data.Login.Contains(edited)) ? password : password+edited;
 
                user.Data.Login = (user.Data.Login.Contains(edited)) ? user.Data.Login.Replace(edited, String.Empty) : user.Data.Login + edited;
                user.Data.Email = (user.Data.Email.Contains(edited)) ? user.Data.Email.Replace(edited, String.Empty) : user.Data.Email + edited;
                user.Data.FirstName = (user.Data.FirstName.Contains(edited)) ? user.Data.FirstName.Replace(edited, String.Empty) : user.Data.FirstName + edited;
                user.Data.LastName = (user.Data.LastName.Contains(edited)) ? user.Data.LastName.Replace(edited, String.Empty) : user.Data.LastName + edited;
                user.Data.Password = new APLPromoter.Server.Entity.User.Password { Old = oldPassword, New = newPassword };
                //user.Data.Role.Id = 25; // Reviewer;
                //TODO - Need service for valid enumeration lists values, like Role

                user = _UserData.SaveIdentity(user);

                this.listener.WriteLine(String.Format("Edit Session valid: {0}", (user.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Edit Client message: {0}", user.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Edit Server message: {0}", user.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Edited User: {0}, {1}, {2}, {3}", user.Data.Name, user.Data.Login, user.Data.Email, user.Data.EditedText)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(user.SessionOk);
                Assert.AreEqual(user.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            Assert.IsTrue(response.SessionOk);
            Assert.IsTrue(list.SessionOk);
            Assert.IsTrue(user.SessionOk);
        }

        //Administrator Adds User Identity (test-pass)...
        [TestMethod, TestCategory("Administration update data pass")]
        public void TEST07_GivenUserIsAuthenticatedAdmin_WhenAddingNewUserIdentity_ThenSuccessStatusRecdAndUserIdentityRecd() {
            Int16 newUserId = 0;
            String uniqueUser = DateTime.Now.ToString("MMdyyyyhhmmss");
            Session<NullT> response = null;
            Session<APLPromoter.Server.Entity.User.Identity> user = null;

            #region Initialize...
            this.listener.WriteLine("Begin - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            try {
                response = _UserData.Initialize(new Session<NullT> { SqlKey = SQLKEYSHARED });
                this.listener.WriteLine(String.Format("Init Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Init SQL Private key: {0}", response.SqlKey)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ServerMessage, String.Empty);
                Assert.AreEqual(response.SqlKey, this.SQLKEYPRIVATE);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            #endregion

            #region Authenticate Admin...
            try {
                response.UserIdentity = new APLPromoter.Server.Entity.User.Identity();
                response.UserIdentity.Login = "Administrator";
                response.UserIdentity.Password = new APLPromoter.Server.Entity.User.Password { Old = "password" };
                response = _UserData.Authenticate(response);
                this.listener.WriteLine(String.Format("Authenticate Session valid: {0}", (response.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate User Authenticated: {0}", (response.Authenticated) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Client message: {0}", response.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Server message: {0}", response.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate SQL Session key: {0}", response.SqlKey)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Authenticate Explorer Planning: {0}", response.UserIdentity.Role.Planning)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(response.SessionOk);
                Assert.AreEqual(response.ServerMessage, String.Empty);
                Assert.IsTrue(response.UserIdentity.Role.Planning.Contains(response.SqlKey.ToUpper()));
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            #endregion

            try { //Administrator Add new Identity...
                user = _UserData.SaveIdentity(new Session<APLPromoter.Server.Entity.User.Identity> {
                    SqlKey = response.SqlKey,
                    UserIdentity = response.UserIdentity,
                    AppOnline = response.AppOnline,
                    Authenticated = response.Authenticated,
                    SqlAuthorization = response.SqlAuthorization,
                    WinAuthorization = response.WinAuthorization,
                    Data = new APLPromoter.Server.Entity.User.Identity {
                        Id = newUserId, /* add a new user */
                        Login = "NewUser" + uniqueUser,
                        FirstName = "New",
                        LastName = "User",
                        Email = "n.user." + uniqueUser + "@aplpromoter.com",
                        Role = new APLPromoter.Server.Entity.User.Role { Id = 25 /* Reviewer */ },
                        Password = new APLPromoter.Server.Entity.User.Password { New = "password", Old=String.Empty }
                    }
                });
                this.listener.WriteLine(String.Format("Add Identity Session valid: {0}", (user.SessionOk) ? "True" : "False")); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Add Identity Client message: {0}", user.ClientMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("Add Identity Server message: {0}", user.ServerMessage)); this.listener.WriteLine(lineBreak);
                this.listener.WriteLine(String.Format("New User: {0}, {1}, {2}, {3}, {4}", user.Data.Name, user.Data.Login, user.Data.Email, user.Data.Role.Name, user.Data.EditedText)); this.listener.WriteLine(lineBreak);

                Assert.IsTrue(user.SessionOk);
                Assert.AreEqual(user.ServerMessage, String.Empty);
            }
            catch (System.Exception ex) {
                this.listener.WriteLine(String.Format("Exception: {0}, {1}", ex.Message, ex.Source)); this.listener.WriteLine(lineBreak);
                Assert.IsTrue(false);
            }
            this.listener.WriteLine("End - " + System.Reflection.MethodInfo.GetCurrentMethod().Name); this.listener.WriteLine(lineBreak);
            Assert.IsTrue(response.SessionOk);
            Assert.IsTrue(user.SessionOk);
        }
    }
}
