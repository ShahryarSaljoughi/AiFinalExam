using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CSP.Interactor
{
    public class Engine
    {
        public async Task StartAsync()
        {
            int k = await FileBoundary.GetKAsync();
            int[,] visibility = await FileBoundary.ReadVisibilityMatrixAsync();
            int[,] communications = await FileBoundary.ReadCommunicationsMatrixAsync();
            
            var solver = new CspSolver(k, visibility, communications);
            var result = solver.Solve();
            await FileBoundary.WriteBackResultAsync(result);
        }
    }
}