using System.Collections.Generic;
using Lumpn.Dungeon;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Lumpn.ZeldaDungeon.Test
{
    [TestFixture]
    public sealed class StateTest
    {
        private static VariableAssignment CreateVariables1()
        {
            return new VariableAssignment();
        }

        private static VariableAssignment CreateVariables2()
        {
            var variables = new VariableAssignment();
            variables.Assign("x", 3);
            return variables;
        }

        private static VariableAssignment CreateVariables3()
        {
            var variables = new VariableAssignment();
            variables.Assign("a", 1);
            variables.Assign("b", 1);
            variables.Assign("c", 1);
            return variables;
        }

        [Test]
        public void EqualsByContent()
        {
            var lookup = new VariableLookup();
            lookup.Resolve("a");
            lookup.Resolve("b");
            lookup.Resolve("c");
            lookup.Resolve("x");

            var variables = CreateVariables3();

            State a = CreateVariables1().ToState(lookup);
            State b = CreateVariables2().ToState(lookup);
            State x = variables.ToState(lookup);
            State y = variables.ToState(lookup);
            State z = CreateVariables3().ToState(lookup);

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
            var lookup = new VariableLookup();
            lookup.Resolve("a");
            lookup.Resolve("b");
            lookup.Resolve("c");
            lookup.Resolve("x");

            var variables = CreateVariables3();

            State a = CreateVariables1().ToState(lookup);
            State b = CreateVariables2().ToState(lookup);
            State x = variables.ToState(lookup);
            State y = variables.ToState(lookup);
            State z = CreateVariables3().ToState(lookup);

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
