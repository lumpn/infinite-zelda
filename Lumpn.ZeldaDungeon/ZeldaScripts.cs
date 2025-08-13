using Lumpn.Dungeon;

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
            return new ItemScript(lookup.Resolve(keys[type]), lookup);
        }

        public static DoorScript CreateDoor(int type, VariableLookup lookup)
        {
            return new DoorScript(lookup.Resolve(keys[type]), lookup, doors[type]);
        }

        public static ColorSwitchScript CreateColorSwitch(int type, VariableLookup lookup)
        {
            return new ColorSwitchScript(lookup.Resolve(switches[type]));
        }

        public static ColorPistonScript CreateColorPiston(int type, int color, VariableLookup lookup)
        {
            return new ColorPistonScript(lookup.Resolve(switches[type]), color, redPistons[type]);
        }

        public static ItemScript CreateTool(int type, VariableLookup lookup)
        {
            return new ItemScript(lookup.Resolve(tools[type]), lookup);
        }

        public static ObstacleScript CreateObstacle(int type, VariableLookup lookup)
        {
            return new ObstacleScript(lookup.Resolve(tools[type]), obstacles[type]);
        }
    }
}
