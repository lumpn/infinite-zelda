using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;

namespace Lumpn.ZeldaDungeon
{
    public static class ZeldaScripts
    {
        private static readonly string[] switches = { "red/blue switch", "train switch" };
        private static readonly string[] redPistons = { "red pistons", "track A" };
        private static readonly string[] bluePistons = { "blue pistons", "track B" };

        private static readonly string[] keys = { "small key", "bomb" };
        private static readonly string[] doors = { "door", "crack" };
        private static readonly string[] tools = { "sword", "shield", "boomerang", "bow", "flippers", "feather", "ember seed", "teleporter", "boss key", "long hook" };
        private static readonly string[] obstacles = { "bush", "trap", "orb", "statue", "water", "gap", "torch", "teleporter", "boss door", "long gap" };

        public static AcquireScript CreateKey(int type, VariableLookup lookup)
        {
            return new AcquireScript(keys[type], lookup);
        }

        public static ConsumeScript CreateDoor(int type, VariableLookup lookup)
        {
            return new ConsumeScript(doors[type], keys[type], lookup);
        }

        public static ToggleScript CreateColorSwitch(int type, VariableLookup lookup)
        {
            return new ToggleScript(switches[type], lookup);
        }

        public static EqualsScript CreateColorPiston(int type, int color, VariableLookup lookup)
        {
            var pistons = (color == 0) ? redPistons : bluePistons;
            return new EqualsScript(color, pistons[type], switches[type], lookup);
        }

        public static AcquireScript CreateTool(int type, VariableLookup lookup)
        {
            return new AcquireScript(tools[type], lookup);
        }

        public static GreaterThanScript CreateObstacle(int type, VariableLookup lookup)
        {
            return new GreaterThanScript(0, obstacles[type], tools[type], lookup);
        }
    }
}
