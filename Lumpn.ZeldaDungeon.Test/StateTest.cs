using System.Collections.Generic;
using Lumpn.Dungeon;
using NUnit.Framework;

namespace Lumpn.ZeldaDungeon.Test
{
    using Variables = Dictionary<VariableIdentifier, int>;

    [TestFixture]
    public sealed class StateTest
    {
        private static Variables CreateVariables1()
        {
            return new Variables();
        }

        private static Variables CreateVariables2()
        {
            var variables = new Variables();
            variables.Add(new VariableIdentifier(42, "x"), 3);
            return variables;
        }

        private static Variables CreateVariables3()
        {
            var variables = new Variables();
            variables.Add(new VariableIdentifier(1, "a"), 1);
            variables.Add(new VariableIdentifier(2, "b"), 1);
            variables.Add(new VariableIdentifier(3, "c"), 1);
            return variables;
        }

        [Test]
        public void EqualsByContent()
        {
            var variables = CreateVariables3();

            State a = new State(CreateVariables1());
            State b = new State(CreateVariables2());
            State x = new State(variables);
            State y = new State(variables);
            State z = new State(CreateVariables3());

            // self equality
            Assert.AreNotSame(x, y);
            Assert.AreNotSame(x, z);
            Assert.AreNotSame(y, z);
            Assert.AreEqual(x, x);
            Assert.AreEqual(y, y);
            Assert.AreEqual(z, z);

            // equality by content
            Assert.AreEqual(x, y);
            Assert.AreEqual(y, x);
            Assert.AreEqual(x, z);
            Assert.AreEqual(z, x);
            Assert.AreEqual(y, z);
            Assert.AreEqual(z, y);

            // more equality by content
            Assert.AreNotSame(x, a);
            Assert.AreNotSame(x, b);
            Assert.AreNotEqual(x, a);
            Assert.AreNotEqual(a, x);
            Assert.AreNotEqual(x, b);
            Assert.AreNotEqual(b, x);
        }

        [Test]
        public void HashCodeEqualsByContent()
        {
            var variables = CreateVariables3();

            State a = new State(CreateVariables1());
            State b = new State(CreateVariables2());
            State x = new State(variables);
            State y = new State(variables);
            State z = new State(CreateVariables3());

            // self equality
            Assert.AreNotSame(x, y);
            Assert.AreNotSame(x, z);
            Assert.AreNotSame(y, z);
            Assert.AreEqual(x.GetHashCode(), x.GetHashCode());
            Assert.AreEqual(y.GetHashCode(), y.GetHashCode());
            Assert.AreEqual(z.GetHashCode(), z.GetHashCode());

            // hash equality by content
            Assert.AreEqual(x.GetHashCode(), y.GetHashCode());
            Assert.AreEqual(y.GetHashCode(), x.GetHashCode());
            Assert.AreEqual(x.GetHashCode(), z.GetHashCode());
            Assert.AreEqual(z.GetHashCode(), x.GetHashCode());
            Assert.AreEqual(y.GetHashCode(), z.GetHashCode());
            Assert.AreEqual(z.GetHashCode(), y.GetHashCode());

            // more hash equality by content
            Assert.AreNotSame(x, a);
            Assert.AreNotSame(x, b);
            Assert.AreNotEqual(x.GetHashCode(), a.GetHashCode());
            Assert.AreNotEqual(a.GetHashCode(), x.GetHashCode());
            Assert.AreNotEqual(x.GetHashCode(), b.GetHashCode());
            Assert.AreNotEqual(b.GetHashCode(), x.GetHashCode());
        }
    }
}
