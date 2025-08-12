using Lumpn.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Lumpn.Mooga.Test
{
    [TestFixture]
    public sealed class BinaryTournamentSelectionTest
    {
        [Test]
        public void SelectSome()
        {
            var individuals = new List<Individual>();
            individuals.Add(new SimpleIndividual(3));
            individuals.Add(new SimpleIndividual(1));
            individuals.Add(new SimpleIndividual(4));
            individuals.Add(new SimpleIndividual(1));
            individuals.Add(new SimpleIndividual(5));
            individuals.Add(new SimpleIndividual(9));
            Assert.AreEqual(6, individuals.Count);

            var random = new SystemRandom(42);
            var selection = new BinaryTournamentSelection(random);
            var resultA = selection.Select(individuals, 0);
            var resultB = selection.Select(individuals, 4);
            var resultC = selection.Select(individuals, 6);
            var resultD = selection.Select(individuals, 10);
            Assert.AreEqual(6, individuals.Count);
            Assert.AreEqual(0, resultA.Count);
            Assert.AreEqual(4, resultB.Count);
            Assert.AreEqual(6, resultC.Count);
            Assert.AreEqual(6, resultD.Count);
        }
    }
}
