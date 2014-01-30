using Microsoft.VisualStudio.TestTools.UnitTesting;
using APLPromoter.Client.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APLPromoter.Client;

namespace APLPromoter.Tests.Client
{
    [TestClass]
    public class ProxyTests
    {
        private String SQLKEYSHARED = "72B9ED08-5D12-48FD-9CF7-56A3CA30E660"; //Shared application key
        private String SQLKEYPRIVATE = "9C8B31D8-ACD5-446A-912E-3019BAF05E6C"; //Private customer key
        private String SQLKEYAPLADMIN = "45F2AE12-1428-481E-8A87-43566914B91A"; //APL Administrator
        [TestMethod]
        public void test_user_client_connection()
        {
            UserClient proxy = new UserClient();
            proxy.Open();

            Session<NullT> response = proxy.Initialize(new Session<NullT> { SqlKey = SQLKEYSHARED });
            response.UserIdentity = new APLPromoter.Client.Entity.User.Identity();
            response.UserIdentity.Login = "Administrator";
            response.UserIdentity.Password = new APLPromoter.Client.Entity.User.Password { Old = "password" };

            var authenticated = proxy.Authenticate(response);

            proxy.Close();


            Assert.IsTrue(response.SessionOk);
            Assert.IsTrue(authenticated.SessionOk);
            //Assert.AreEqual(response.ServerMessage, String.Empty);
            //Assert.AreEqual(response.SqlKey, this.SQLKEYPRIVATE);

        }

        [TestMethod]
        public void test_analytic_client_connection()
        {
            AnalyticClient proxy = new AnalyticClient();
            proxy.Open();

            Session<List<APLPromoter.Client.Entity.Analytic.Identity>> response = proxy.LoadList(new Session<APLPromoter.Client.Entity.NullT> { SqlKey = SQLKEYAPLADMIN });

            proxy.Close();


            Assert.IsTrue(response.SessionOk);
            //Assert.AreEqual(response.ServerMessage, String.Empty);
            //Assert.AreEqual(response.SqlKey, this.SQLKEYPRIVATE);
        }
    }
}
