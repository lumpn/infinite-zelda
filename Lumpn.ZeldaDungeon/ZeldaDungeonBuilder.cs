using Lumpn.Dungeon;

namespace Lumpn.ZeldaDungeon
{
    public sealed class ZeldaDungeonBuilder
    {
        private readonly CrawlerBuilder crawlerBuilder = new CrawlerBuilder();

        private readonly VariableLookup lookup = new VariableLookup();

        public VariableLookup Lookup { get { return lookup; } }

        public void AddDirectedTransition(int start, int end, Script script)
        {
            crawlerBuilder.AddDirectedTransition(start, end, script);
        }

        public void AddUndirectedTransition(int loc1, int loc2, Script script)
        {
            crawlerBuilder.AddUndirectedTransition(loc1, loc2, script);
        }

        public void AddScript(int location, Script script)
        {
            crawlerBuilder.AddScript(location, script);
        }

        public Crawler Build()
        {
            return crawlerBuilder.Build();
        }
    }
}
