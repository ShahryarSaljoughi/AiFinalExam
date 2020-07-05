using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CSP.Interactor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSP.Test
{
    [TestClass]
    public class EngineTests
    {

        [TestMethod]
        public async Task EngineMustWork()
        {
            var engine = new Engine();
            await engine.StartAsync();
        }
    }
}
