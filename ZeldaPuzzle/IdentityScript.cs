namespace Lumpn.ZeldaPuzzle
{
    public sealed class IdentityScript : ZeldaScript
    {
        public static readonly IdentityScript instance = new IdentityScript();

        public State Execute(State state)
        {
            return state;
        }

        public void Express(DotTransitionBuilder builder)
        {
            builder.SetLabel("");
        }
    }
}
