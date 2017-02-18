using System;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OneSet.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void Test_AutomapperConfig()
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Automapper();
            Mapper.AssertConfigurationIsValid();
        }
    }
}
