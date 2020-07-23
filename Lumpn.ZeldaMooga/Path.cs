using Lumpn.Mooga;
using Lumpn.Utils;
using System.Collections.Generic;

namespace Lumpn.ZeldaMooga
{

    public sealed class Path
    {

        public Path(Path prefix, Step step)
        {
            this.prefix = prefix;
            this.step = step;
        }

        public Step step()
        {
            return step;
        }

        public boolean endsAtLocation(int location)
        {
            return step.location().id() == location;
        }

        public int length()
        {
            if (prefix == null) return 0;
            return prefix.length() + 1;
        }

        private readonly Path prefix;
        private readonly Step step;
    }
}