namespace Lumpn.ZeldaLayout
{
    public sealed class Position
    {
        public enum Direction
        {
            NORTH, SOUTH, // y movement
            EAST, WEST, // x movement
            UP, DOWN, // z movement
            NONE
        }

        public static Direction getDirection(Position from, Position to)
        {
            if (from.x < to.x) return Direction.EAST;
            if (from.x > to.x) return Direction.WEST;
            if (from.y < to.y) return Direction.NORTH;
            if (from.y > to.y) return Direction.SOUTH;
            if (from.z < to.z) return Direction.UP;
            if (from.z > to.z) return Direction.DOWN;
            return Direction.NONE;
        }

        public static int getDistance(Position from, Position to)
        {
            return Math.abs(to.x - from.x) + Math.abs(to.y - from.y) + Math.abs(to.z - from.z);
        }

        public Position(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int getZ()
        {
            return z;
        }

        public List<Position> getNeighbors()
        {
            List<Position> result = new ArrayList<Position>();
            result.add(new Position(x + 1, y, z));
            result.add(new Position(x - 1, y, z));
            result.add(new Position(x, y + 1, z));
            result.add(new Position(x, y - 1, z));
            result.add(new Position(x, y, z + 1));
            result.add(new Position(x, y, z - 1));
            return result;
        }

        @Override
        public int hashCode()
        {
            final int prime = 31;
            int result = 1;
            result = prime * result + x;
            result = prime * result + y;
            result = prime * result + z;
            return result;
        }

        @Override
        public boolean equals(Object obj)
        {
            if (this == obj) return true;
            if (obj == null) return false;
            if (!(obj instanceof Position)) return false;
            Position other = (Position)obj;
            if (x != other.x) return false;
            if (y != other.y) return false;
            if (z != other.z) return false;
            return true;
        }

        @Override
        public String toString()
        {
            return String.format("(%d, %d, %d)", x, y, z);
        }

        /**
         * Position in grid.
         */
        private readonly int x, y, z;

    }
}