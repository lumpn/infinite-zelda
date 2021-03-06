﻿using System.Linq;
using System.Collections.Generic;
using Lumpn.Dungeon;
using Lumpn.Mooga;
using Lumpn.ZeldaDungeon;
using Lumpn.Profiling;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaEnvironment : Environment
    {
        private readonly VariableAssignment[] initialVariables;
        private readonly int maxSteps;

        public ZeldaEnvironment(VariableAssignment[] initialVariables, int maxSteps)
        {
            this.initialVariables = initialVariables;
            this.maxSteps = maxSteps;
        }

        public Individual Evaluate(Genome g)
        {
            ZeldaGenome genome = (ZeldaGenome)g;

            // build puzzle
            var builder = new ZeldaDungeonBuilder();
            Profiler.BeginSample("Express");
            genome.Express(builder);
            Profiler.EndSample();
            var crawler = builder.Build();

            // build initial states
            var initialStates = new List<State>();
            foreach (var variableAssignment in initialVariables)
            {
                var state = variableAssignment.ToState(builder.Lookup);
                initialStates.Add(state);
            }

            // crawl puzzle
            Profiler.BeginSample("Crawl");
            var terminalSteps = crawler.Crawl(initialStates, maxSteps);
            Profiler.EndSample();

            // evaluate puzzle
            Profiler.BeginSample("Assess");
            int numSteps = crawler.DebugGetSteps().Count();
            int numDeadEnds = ErrorCounter.CountDeadEnds(crawler);
            int shortestPathLength = PathFinder.CalcShortestPathLength(terminalSteps);
            double revisitFactor = PathFinder.CalcRevisitFactor(crawler);
            double branchFactor = PathFinder.CalcBranchFactor(crawler);
            Profiler.EndSample();

            // create individual
            return new ZeldaIndividual(genome, crawler, numSteps, numDeadEnds, shortestPathLength, revisitFactor, branchFactor);
        }
    }
}