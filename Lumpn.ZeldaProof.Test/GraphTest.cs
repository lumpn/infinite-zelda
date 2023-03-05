using NUnit.Framework;

namespace Lumpn.ZeldaProof.Test
{
    [TestFixture]
    public sealed class GraphTest
    {
        [Test]
        public void Trivial()
        {
            var builder = new GraphBuilder();
            builder.addTransition(0, 1);

            var graph = builder.build();
            graph.print(System.Console.Out);

            var validator = new GraphValidator();
            var result = validator.validate(graph);
            Assert.IsTrue(result);
        }

        [Test]
        public void TrivialNoPath()
        {
            var builder = new GraphBuilder();

            var graph = builder.build();
            graph.print(System.Console.Out);

            var validator = new GraphValidator();
            var result = validator.validate(graph);
            Assert.IsFalse(result);
        }

        [Test]
        public void TrivialNoPath2()
        {
            var builder = new GraphBuilder();
            builder.addTransition(0, 2);

            var graph = builder.build();
            graph.print(System.Console.Out);

            var validator = new GraphValidator();
            var result = validator.validate(graph);
            Assert.IsFalse(result);
        }

        [Test]
        public void Basic()
        {
            var builder = new GraphBuilder();
            builder.addItem(0, "A");
            builder.addTransition(0, 1, "A");

            var graph = builder.build();
            graph.print(System.Console.Out);

            var validator = new GraphValidator();
            var result = validator.validate(graph);
            Assert.IsTrue(result);
        }

        [Test]
        public void MissingItem()
        {
            var builder = new GraphBuilder();
            builder.addTransition(0, 1, "A");

            var graph = builder.build();
            graph.print(System.Console.Out);

            var validator = new GraphValidator();
            var result = validator.validate(graph);
            Assert.IsFalse(result);
        }

        [Test]
        public void ExtraItem()
        {
            var builder = new GraphBuilder();
            builder.addItem(0, "A");
            builder.addTransition(0, 1);

            var graph = builder.build();
            graph.print(System.Console.Out);

            var validator = new GraphValidator();
            var result = validator.validate(graph);
            Assert.IsFalse(result);
        }

        [Test]
        public void Sequence()
        {
            var builder = new GraphBuilder();
            builder.addItem(0, "A");
            builder.addTransition(0, 1, "B");
            builder.addTransition(0, 2, "A");
            builder.addItem(2, "B");

            var graph = builder.build();
            graph.print(System.Console.Out);

            var validator = new GraphValidator();
            var result = validator.validate(graph);
            Assert.IsTrue(result);
        }
    }
}
