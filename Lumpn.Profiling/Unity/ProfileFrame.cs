using System.Collections.Generic;
using System.IO;

namespace Lumpn.Profiling.Unity
{
    public sealed class ProfileFrame
    {
        private readonly List<ProfileThread> threads = new List<ProfileThread>();
        private double msStartTime;
        private float msFrame;

        public ProfileFrame(double msStartTime, float msFrame)
        {
            this.msStartTime = msStartTime;
            this.msFrame = msFrame;
        }

        public ProfileFrame(BinaryReader reader)
        {
            msStartTime = reader.ReadDouble();
            msFrame = reader.ReadSingle();

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                threads.Add(new ProfileThread(reader));
            }
        }

        public void Add(ProfileThread thread)
        {
            threads.Add(thread);
        }

        public void Write(BinaryWriter writer)
        {
            System.Console.WriteLine("start {0}, ms {1}", msStartTime, msFrame);
            writer.Write(msStartTime);
            writer.Write(msFrame);

            System.Console.WriteLine("threads {0}", threads.Count);
            writer.Write(threads.Count);
            foreach (var thread in threads)
            {
                thread.Write(writer);
            };
        }
    }
}
