using System.Collections.Generic;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Lumpn.Dungeon.Test
{
    [TestFixture]
    public class StateTest
    {
        [Test]
        public void AllocateSetGet()
        {
            var lookup = new VariableLookup();
            var keyId = lookup.Resolve("key");
            var keyStateId = lookup.Unique("key state");
            var doorStateId = lookup.Unique("door state");

            var numVariables = lookup.numVariables;
            Assert.AreEqual(3, numVariables);

            var memory = new Memory(numVariables, 10);
            var state = memory.Allocate();

            Assert.AreEqual(0, state.Get(keyId));
            Assert.AreEqual(0, state.Get(keyStateId));
            Assert.AreEqual(0, state.Get(doorStateId));

            state.Set(keyId, 1);
            state.Set(keyStateId, 2);
            state.Set(doorStateId, 3);

            Assert.AreEqual(1, state.Get(keyId));
            Assert.AreEqual(2, state.Get(keyStateId));
            Assert.AreEqual(3, state.Get(doorStateId));
        }

        [Test]
        public void Deduplicate()
        {
            var lookup = new VariableLookup();
            var keyId = lookup.Resolve("key");
            var keyStateId = lookup.Unique("key state");
            var doorStateId = lookup.Unique("door state");

            var numVariables = lookup.numVariables;
            Assert.AreEqual(3, numVariables);

            var states = new HashSet<State>(StateEqualityComparer.Default);
            var memory = new Memory(lookup.numVariables, 10);

            var stateA = memory.Allocate();
            stateA.Set(keyId, 1);
            stateA.Set(keyStateId, 2);
            stateA.Set(doorStateId, 3);
            Assert.IsTrue(states.Add(stateA));

            var stateB = memory.Allocate();
            stateB.Set(keyId, 4);
            stateB.Set(keyStateId, 5);
            stateB.Set(doorStateId, 6);
            Assert.IsTrue(states.Add(stateB));

            var stateC = memory.Allocate();
            stateC.Set(keyId, 1);
            stateC.Set(keyStateId, 2);
            stateC.Set(doorStateId, 3);
            Assert.IsFalse(states.Add(stateC));
        }
    }
}
