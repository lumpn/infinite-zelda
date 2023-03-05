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
            graph.Print(System.Console.Out);

            var result = graph.Validate();
            Assert.IsTrue(result);
        }

        [Test]
        public void Trivial2()
        {
            var graph = new Graph(1);
            graph.AddTransition(0, 1);
            graph.Print(System.Console.Out);

            var result = graph.Validate();
            Assert.IsTrue(result);
        }

        [Test]
        public void TrivialNoPath()
        {
            var graph = new Graph(1);
            graph.Print(System.Console.Out);

            var result = graph.Validate();
            Assert.IsFalse(result);
        }

        [Test]
        public void TrivialNoPath2()
        {
            var graph = new Graph(1);
            graph.AddTransition(0, 2);
            graph.Print(System.Console.Out);

            var result = graph.Validate();
            Assert.IsFalse(result);
        }

        [Test]
        public void Basic()
        {
            var graph = new Graph(1);
            graph.AddItem(0, "A");
            graph.AddTransition(0, 1, "A");
            graph.Print(System.Console.Out);

            var result = graph.Validate();
            Assert.IsTrue(result);
        }

        [Test]
        public void MissingItem()
        {
            var graph = new Graph(1);
            graph.AddTransition(0, 1, "A");
            graph.Print(System.Console.Out);

            var result = graph.Validate();
            Assert.IsFalse(result);
        }

        [Test]
        public void ExtraItem()
        {
            var graph = new Graph(1);
            graph.AddItem(0, "A");
            graph.AddTransition(0, 1);
            graph.Print(System.Console.Out);

            var result = graph.Validate();
            Assert.IsFalse(result);
        }

        [Test]
        public void Sequence()
        {
            var graph = new Graph(1);
            graph.AddItem(0, "A");
            graph.AddTransition(0, 1, "B");
            graph.AddTransition(0, 2, "A");
            graph.AddItem(2, "B");
            graph.Print(System.Console.Out);

            var result = graph.Validate();
            Assert.IsTrue(result);
        }
    }
}
