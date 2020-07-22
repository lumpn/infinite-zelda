using Lumpn.Dungeon;

namespace Lumpn.ZeldaDungeon
{
    public static class ZeldaScripts
    {
        private static readonly string switchName = "red/blue\\nswitch";

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

        public static ColorSwitchScript CreateColorSwitch(VariableLookup lookup)
        {
            return new ColorSwitchScript(lookup.Resolve(switchName));
        }

        public static ColorPistonScript CreateRedPiston(VariableLookup lookup)
        {
            return new ColorPistonScript(lookup.Resolve(switchName), ZeldaStates.SwitchRed);
        }

        public static ColorPistonScript CreateBluePiston(VariableLookup lookup)
        {
            return new ColorPistonScript(lookup.Resolve(switchName), ZeldaStates.SwitchBlue);
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
