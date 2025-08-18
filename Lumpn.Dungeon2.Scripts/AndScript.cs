namespace Lumpn.Dungeon2.Scripts
{
    /// an AND blocker
    public sealed class AndScript : Script
    {
        private readonly Script a, b;

        public string Name { get { return $"{a.Name} AND {b.Name}"; } }

        public AndScript(Script a, Script b)
        {
            this.a = a;
            this.b = b;
        }

        public ScriptResult Execute(State state, StateBuilder builder)
        {
            var resultA = a.Execute(state, builder);
            switch (resultA)
            {
                case ScriptResult.Fail:
                    return ScriptResult.Fail;
                case ScriptResult.Pass:
                    return b.Execute(state, builder);
                case ScriptResult.Modify:
                    var tmpState = builder.GetState();
                    return b.Execute(tmpState, builder); // TODO Jonas: technically not correct. need to copy tmpState first. stackalloc?
            }
            return ScriptResult.Fail;
        }
    }
}
