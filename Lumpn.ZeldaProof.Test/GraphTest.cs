using NUnit.Framework;

namespace Lumpn.ZeldaProof.Test
{
    [TestFixture]
    public sealed class GraphTest
    {
        [Test]
        public void Trivial()
        {
            var graph = new Graph(0);
            graph.print(System.Console.Out);

            var result = graph.validate();
            Assert.IsTrue(result);
        }

        [Test]
        public void Trivial2()
        {
            var graph = new Graph(1);
            graph.addTransition(0, 1);
            graph.print(System.Console.Out);

            var result = graph.validate();
            Assert.IsTrue(result);
        }

        [Test]
        public void TrivialNoPath()
        {
            var graph = new Graph(1);
            graph.print(System.Console.Out);

            var result = graph.validate();
            Assert.IsFalse(result);
        }

        [Test]
        public void TrivialNoPath2()
        {
            var graph = new Graph(1);
            graph.addTransition(0, 2);
            graph.print(System.Console.Out);

            var result = graph.validate();
            Assert.IsFalse(result);
        }

        [Test]
        public void Basic()
        {
            var graph = new Graph(1);
            graph.addItem(0, "A");
            graph.addTransition(0, 1, "A");
            graph.print(System.Console.Out);

            var result = graph.validate();
            Assert.IsTrue(result);
        }

        [Test]
        public void MissingItem()
        {
            var graph = new Graph(1);
            graph.addTransition(0, 1, "A");
            graph.print(System.Console.Out);

            var result = graph.validate();
            Assert.IsFalse(result);
        }

        [Test]
        public void ExtraItem()
        {
            var graph = new Graph(1);
            graph.addItem(0, "A");
            graph.addTransition(0, 1);
            graph.print(System.Console.Out);

            var result = graph.validate();
            Assert.IsFalse(result);
        }

        [Test]
        public void Sequence()
        {
            var graph = new Graph(1);
            graph.addItem(0, "A");
            graph.addTransition(0, 1, "B");
            graph.addTransition(0, 2, "A");
            graph.addItem(2, "B");
            graph.print(System.Console.Out);

            var result = graph.validate();
            Assert.IsTrue(result);
        }
    }
}
