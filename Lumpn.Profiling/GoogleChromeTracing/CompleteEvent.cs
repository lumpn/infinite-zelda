using System.IO;

namespace Lumpn.Profiling.GoogleChromeTracing
{
    public sealed class CompleteEvent
    {
        private readonly int processId;
        private readonly int threadId;
        private readonly double timestampMS;
        private readonly float durationMS;
        private readonly string name;

        public CompleteEvent(int processId, int threadId, double timestampMS, float durationMS, string name)
        {
            this.processId = processId;
            this.threadId = threadId;
            this.timestampMS = timestampMS;
            this.durationMS = durationMS;
            this.name = name;
        }

        public void WriteTo(TextWriter writer)
        {
            writer.Write("{ \"pid\":");
            writer.Write(processId);
            writer.Write(", \"tid\":");
            writer.Write(threadId);
            writer.Write(", \"ts\":");
            writer.Write(timestampMS);
            writer.Write(", \"dur\":");
            writer.Write(durationMS);
            writer.Write(", \"name\":\"");
            writer.Write(name);
            writer.Write("\", \"ph\":\"X\" }");
        }
    }
}
