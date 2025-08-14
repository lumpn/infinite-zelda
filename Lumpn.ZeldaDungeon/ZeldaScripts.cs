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
        private static readonly string[] tools = { "sword", "shield", "boomerang", "bow", "flippers", "feather", "ember seed", "teleporter", "boss key" };
        private static readonly string[] obstacles = { "bush", "trap", "orb", "statue", "water", "gap", "torch", "teleporter", "boss door" };

        public static ItemScript CreateKey(int type, VariableLookup lookup)
        {
            return new ItemScript(keys[type], lookup);
        }

        public static DoorScript CreateDoor(int type, VariableLookup lookup)
        {
            return new DoorScript(doors[type], keys[type], lookup);
        }

        public static SwitchScript CreateColorSwitch(int type, VariableLookup lookup)
        {
            return new SwitchScript(switches[type], lookup);
        }

        public static BlockerScript CreateColorPiston(int type, int color, VariableLookup lookup)
        {
            var pistons = (color == 0) ? redPistons : bluePistons;
            return new BlockerScript(color, pistons[type], switches[type], lookup);
        }

        public static ItemScript CreateTool(int type, VariableLookup lookup)
        {
            return new ItemScript(tools[type], lookup);
        }

        public static ObstacleScript CreateObstacle(int type, VariableLookup lookup)
        {
            return new ObstacleScript(obstacles[type], tools[type], lookup);
        }
    }
}
