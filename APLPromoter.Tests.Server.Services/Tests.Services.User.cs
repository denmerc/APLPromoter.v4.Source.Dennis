using APLPromoter.Server.Data;
using APLPromoter.Server.Entity;
using APLPromoter.Server.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLPromoter.Tests.Server.Services
{
    [TestClass]
    public class Tests
    {

        private String SQLKEYSHARED = "72B9ED08-5D12-48FD-9CF7-56A3CA30E660"; //Shared application key
        private String SQLKEYPRIVATE = "9C8B31D8-ACD5-446A-912E-3019BAF05E6C"; //Private customer key

        [TestMethod, TestCategory("Analytic.Services")]
        public void UserService_Initialize()
        {
            UserService s = new UserService();

            Session<NullT> response = s.Initialize(new Session<NullT> { SqlKey = SQLKEYSHARED });


            Assert.IsTrue(response.SessionOk);
            Assert.AreEqual(response.ServerMessage, String.Empty);
            Assert.AreEqual(response.SqlKey, this.SQLKEYPRIVATE);

        }
    }
}
