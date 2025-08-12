using System.IO;

namespace Lumpn.Profiling.GoogleChromeTracing
{
    public struct CompleteEvent
    {
        private readonly int processId;
        private readonly int threadId;
        private readonly long timestamp; // [microseconds]
        private readonly long duration; // [microseconds]
        private readonly string name;

        public CompleteEvent(int processId, int threadId, long timestamp, long duration, string name)
        {
            this.processId = processId;
            this.threadId = threadId;
            this.timestamp = timestamp;
            this.duration = duration;
            this.name = name;
        }

        public void WriteTo(TextWriter writer)
        {
            writer.Write("{ \"pid\":");
            writer.Write(processId);
            writer.Write(", \"tid\":");
            writer.Write(threadId);
            writer.Write(", \"ts\":");
            writer.Write(timestamp);
            writer.Write(", \"dur\":");
            writer.Write(duration);
            writer.Write(", \"name\":\"");
            writer.Write(name);
            writer.Write("\", \"ph\":\"X\" }");
        }
    }
}
