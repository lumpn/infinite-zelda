using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Lumpn.ZeldaProof.Test
{
    [TestFixture]
    public sealed class GraphTest
    {
        [Test]
        public void CreateGraph()
        {
            var builder = new GraphBuilder();
            builder.addItem(0, "A");
            builder.addTransition(0, 1, "A");

            var graph = builder.build();
            var verifier = new GraphValidator();
            
            var result = verifier.Verify(graph);
            Assert.IsTrue(result);
        }
    }
}
