using System;
using System.Threading.Tasks;
using CSP.Interactor;

namespace CSP
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var engine = new Engine();
            await engine.StartAsync();
        }
    }
}
