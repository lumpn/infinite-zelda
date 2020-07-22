using System.Collections.Generic;

namespace Lumpn.ZeldaLayout
{
    /// Intermediate state of the search for a valid lowering of the schedule.
    /// Consists of an intermediate grid, its cost, and a schedule of remaining
    /// transitions to lower.
    public sealed class State
    {
        private const int roomCost = 100;
        private const int scriptCost = 10;
        private const int stairsCost = 5;

        /// Each room cost something, each door cost at least as much 
        /// as a transition in schedule!
        private static int calcCost(Grid grid)
        {
            int result = 0;
            result += grid.numRooms() * roomCost;
            result += grid.numScripts() * scriptCost;
            result += grid.numStairs() * stairsCost;
            return result;
        }

        /// Estimate must be lower bound of final cost!
        private static int calcEstimate(Grid grid, ImmutableList<Transition> schedule)
        {

            // find set of scheduled rooms
            Set<RoomIdentifier> scheduledRooms = new HashSet<RoomIdentifier>();
            for (Transition transition : schedule)
            {
                scheduledRooms.add(transition.getSource());
                scheduledRooms.add(transition.getDestination());
            }

            // calculate number of missing rooms
            int missing = 0;
            for (RoomIdentifier room : scheduledRooms)
            {
                if (!grid.containsRoom(room)) missing++;
            }

            // calculate result
            int result = 0;
            result += missing * roomCost; // at least these cost will occur for creating rooms
            result += schedule.size() * scriptCost; // at least these scripts will be created
            return result;
        }

        public State(Grid grid, ImmutableList<Transition> schedule)
        {
            this.grid = grid;
            this.schedule = schedule;
            this.cost = calcCost(grid);
            this.estimate = calcEstimate(grid, schedule);
        }

        public Grid getGrid()
        {
            return grid;
        }

        public bool scheduleIsEmpty()
        {
            return schedule.isEmpty();
        }

        public int getCost()
        {
            return cost;
        }

        public int getEstimate()
        {
            return estimate;
        }

        public List<State> getNeighbors()
        {
            // implement transition
            List<State> result = new List<State>();
            foreach (Transition transition in schedule)
            {
                // remove from schedule
                List<Transition> tmpSchedule = schedule.toList();
                tmpSchedule.remove(transition);
                ImmutableList<Transition> nextSchedule = new ImmutableArrayList<Transition>(tmpSchedule);

                // implement transition
                List<Grid> implementations = grid.implement(transition);
                for (Grid implementation : implementations)
                {
                    result.add(new State(implementation, nextSchedule));
                }
            }

            return result;
        }

        public int hashCode()
        {
            final int prime = 31;
            int result = 1;
            result = prime * result + grid.hashCode();
            result = prime * result + schedule.hashCode();
            return result;
        }

        public boolean equals(Object obj)
        {
            if (this == obj) return true;
            if (obj == null) return false;
            if (!(obj instanceof State)) return false;
            State other = (State)obj;
            if (!grid.equals(other.grid)) return false;
            if (!schedule.equals(other.schedule)) return false;
            return true;
        }

        private readonly Grid grid;

        private readonly List<Transition> schedule;

        private readonly int cost, estimate;
    }
}