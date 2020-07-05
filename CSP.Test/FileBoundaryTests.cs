using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSP.Test
{
    [TestClass]
    public class FileBoundaryTests
    {
        [TestMethod]
        public async Task ReadVisibilityMustWork()
        {
            var visibility = await FileBoundary.ReadVisibilityMatrixAsync();
            var expected = new int[6, 2]
            {
                {1, 1},
                {0, 1},
                {0, 1},
                {0, 1},
                {1, 1},
                {1, 0},
            };
            Assert.IsTrue(
                Enumerable.Range(0, 6)
                    .Zip(Enumerable.Range(0, 2), (first, second) => (first, second))
                    .All(pair =>
                        visibility[pair.first, pair.second] == expected[pair.first, pair.second]));
        }

        [TestMethod]
        public async Task ReadKMustWork()
        {
            var k = await FileBoundary.GetKAsync();
            Assert.AreEqual(k, 2);
        }

        [TestMethod]
        public async Task ReadCommunicationsMustWork()
        {
            var communications = await FileBoundary.ReadCommunicationsMatrixAsync();
            var expected = new int[6, 6]
            {
                {0,0,0,0,0,1},
                {0,0,0,1,0,1},
                {0,0,0,1,1,0},
                {0,1,1,0,1,0},
                {0,0,1,1,0,0},
                {1,1,0,0,0,0},
            };
            Assert.IsTrue(
                Enumerable.Range(0, 6)
                    .Zip(Enumerable.Range(0, 6), (first, second) => (first, second))
                    .All(pair =>
                        communications[pair.first, pair.second] == expected[pair.first, pair.second]));
        }
    }
}