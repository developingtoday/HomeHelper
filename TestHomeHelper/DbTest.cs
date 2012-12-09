using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Repository.Concret;
using HomeHelper.Utils;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace TestHomeHelper
{
    [TestClass]
    public class DbTest
    {
        [TestMethod]
        public void TestCreateOrGetDatabase()
        {
        
            var output=DbUtils.InitializeDb();
            Assert.AreEqual(output,"Succes");
        }

        [TestMethod]
        public void TestCreateOrUpdateUtilitate()
        {
            IRepository<Utilitati> rep = new UtilitatiRepository();
            var utilitate = new Utilitati();
        }

    }
}
