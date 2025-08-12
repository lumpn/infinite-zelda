namespace Lumpn.Mooga.Test
{
    public sealed class ParetoEnvironment : Environment
    {
        public Individual Evaluate(Genome genome)
        {
            var g = (ParetoGenome)genome;
            return new ParetoIndividual(g.value1, g.value2, g.value3, g);
        }
    }
}
