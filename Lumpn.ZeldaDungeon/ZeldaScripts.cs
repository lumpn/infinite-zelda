using Lumpn.Dungeon;

namespace Lumpn.ZeldaDungeon
{
    public static class ZeldaScripts
    {
        private static readonly string[] switches = { "red/blue\\nswitch", "yellow/green\\nswitch" };
        private static readonly string[] redPistons = { "red\\npistons", "yellow\\npistons" };
        private static readonly string[] bluePistons = { "blue\\npistons", "green\\npistons" };

        private static readonly string[] keys = { "small\\nkey", "bomb" };
        private static readonly string[] doors = { "door", "crack" };
        private static readonly string[] tools = { "sword", "shield", "boomerang", "bow", "flippers", "feather" };
        private static readonly string[] obstacles = { "bush", "trap", "orb", "statue", "water", "gap" };

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

        public static ColorPistonScript CreateRedPiston(int type, VariableLookup lookup)
        {
            return new ColorPistonScript(lookup.Resolve(switches[type]), ZeldaStates.SwitchRed, redPistons[type]);
        }

        public static ColorPistonScript CreateBluePiston(int type, VariableLookup lookup)
        {
            return new ColorPistonScript(lookup.Resolve(switches[type]), ZeldaStates.SwitchBlue, bluePistons[type]);
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
