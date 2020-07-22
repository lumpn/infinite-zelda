namespace Lumpn.Mooga.Test
{
    public sealed class SimpleEnvironment : Environment
    {
        public Individual Evaluate(Genome genome)
        {
            var g = (SimpleGenome)genome;
            return new SimpleIndividual(g.value, g);
        }
    }
}
