using System;
using Lumpn.Mooga;
using Lumpn.Utils;

public sealed class ZeldaMooga
{
    public static void Main(string[] args)
    {
        IRandom random = new SystemRandom(42);

        ZeldaConfiguration configuration = new ZeldaConfiguration();

        ZeldaGenomeFactory factory = new ZeldaGenomeFactory(configuration, random);

        //State initialState = new State(Collections.< VariableIdentifier, Integer > emptyMap());
        ZeldaEnvironment environment = new ZeldaEnvironment(initialState, 10000);

        ZeldaGenome example = factory.CreateGenome();
        ZeldaIndividual individual = environment.evaluate(example);
        System.out.println("test: " + individual);

        ElitistEvolution evolution = new ElitistEvolution(100, 1000, factory, environment);

        List<Genome> genomes = evolution.initialize();

        // TODO: replace fixed weight multirank optimization by dynamic randomized weighting
        // i.e. in some generations prefer some attribute over others

        // evolve
        for (int i = 0; i < 1000; i++)
        {
            System.out.println("gen " + i);
            genomes = evolution.evolve(genomes, random);

            // target reached?
            ZeldaIndividual best = (ZeldaIndividual)evolution.getBest();
            if (best != null && best.getScore(7) == 20) break;
        }

        // refine
        for (int i = 0; i < 50; i++)
        {
            System.out.println("refine " + i);
            genomes = evolution.evolve(genomes, random);
        }

        ZeldaIndividual best = (ZeldaIndividual)evolution.getBest();
        System.out.println(best.toString());

        ZeldaPuzzle puzzle = best.puzzle();
        DotBuilder builder = new DotBuilder();
        puzzle.express(builder);

        // TODO: output genome to puzzle unit test (puzzle building statements)
    }
}
