using System.IO;
using NUnit.Framework;

namespace Lumpn.Profiling.UnityProfileAnalyzer.Test
{
    [TestFixture]
    public sealed class ProfileDataTest
    {
        private const string filename = "w:\\capture.pdata";

        [Test]
        public void LoadProfileData()
        {
            using (var output = File.Create("foo.pdata"))
            {
                using (var writer = new BinaryWriter(output))
                {
                    using (var stream = File.OpenRead(filename))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            var data = ProfileData.ReadFrom(reader);
                            data.WriteTo(writer);
                        }
                    }
                }
            }
        }
    }
}
