using Lumpn.Dungeon;
using Lumpn.Dungeon.Scripts;
using Lumpn.ZeldaDungeon;
using NUnit.Framework;
using System.IO;
using System.Linq;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace ZeldaPuzzle.Test
{
    [TestFixture]
    public sealed class OracleSeasons
    {
        private static readonly VariableAssignment emptyVariables = new VariableAssignment();

        private const int maxSteps = 10000;

        private const int smallKey = 0;
        private const int bomb = 1;
        private const int fireSeed = 6;
        private const int teleporter = 7;
        private const int bossKey = 8;
        private const int train = 1;

        [Test]
        public void GnarledRoot()
        {
            // ---- legend ----
            // K: small key
            // BK: boss key
            // H: hint
            // M: map
            // Cp: compass
            // B: bomb
            // GS: gasha seed
            // F: fire seed
            // SS: seed satchel
            // Sw: switch
            // Es: essence
            // Tp: teleporter
            //
            // ---- layout ----
            //
            //        SS,F -- Tp
            //                |
            //               (K)
            //                |
            //               (B)
            //                +---+              + ----- Es
            //       +-- Sw,Cp    |              |
            // GS -- + --(b)-- B -+- K          (BK)
            //       |                           |
            //       + --(a)             + ------+
            //            |              |
            // BK --(F)-- Tp        H   (F)
            //            |         |    |
            //            M --(K)-- + -- K
            //                      |
            //                      |
            //
            // ---- mission ----
            //
            //                 7Tp,F
            //                  |
            //                 (K)
            //                  |
            //                  6
            //                  |
            //                 (B)        0
            //        Sw        |         |
            //        4 --(b)-- 5B,K     (BK)
            //        |                   |
            //        + --(a)             9
            //             |              |
            // 8BK --(F)-- 3Tp            (F)
            //             |              |
            //             + --(K)-- 1 -- 2K
            //                       |
            //                       |

            var builder = new ZeldaDungeonBuilder();
            var lookup = builder.Lookup;
            builder.AddUndirectedTransition(1, 2, NoOpScript.instance);
            builder.AddScript(2, ZeldaScripts.CreateKey(smallKey, lookup));
            builder.AddUndirectedTransition(1, 3, ZeldaScripts.CreateDoor(smallKey, lookup));
            builder.AddUndirectedTransition(3, 4, ZeldaScripts.CreateColorPiston(train, 0, lookup));
            builder.AddScript(4, ZeldaScripts.CreateColorSwitch(train, lookup));
            builder.AddUndirectedTransition(4, 5, ZeldaScripts.CreateColorPiston(train, 1, lookup));
            builder.AddScript(5, ZeldaScripts.CreateKey(bomb, lookup));
            builder.AddScript(5, ZeldaScripts.CreateKey(smallKey, lookup));
            builder.AddUndirectedTransition(5, 6, ZeldaScripts.CreateDoor(bomb, lookup));
            builder.AddUndirectedTransition(6, 7, ZeldaScripts.CreateDoor(smallKey, lookup));
            builder.AddScript(7, ZeldaScripts.CreateTool(teleporter, lookup));
            builder.AddScript(7, ZeldaScripts.CreateTool(fireSeed, lookup));
            builder.AddUndirectedTransition(7, 3, ZeldaScripts.CreateObstacle(teleporter, lookup));
            builder.AddUndirectedTransition(3, 8, ZeldaScripts.CreateObstacle(fireSeed, lookup));
            builder.AddScript(8, ZeldaScripts.CreateTool(bossKey, lookup));
            builder.AddUndirectedTransition(2, 9, ZeldaScripts.CreateObstacle(fireSeed, lookup));
            builder.AddUndirectedTransition(9, 0, ZeldaScripts.CreateObstacle(bossKey, lookup));

            var crawler = builder.Build();
            var initialState = emptyVariables.ToState(lookup);
            var trace = crawler.Crawl(new[] { initialState }, maxSteps);

            Assert.NotZero(trace.CountSteps(0));

            PrintMission(crawler, lookup, 1);
            trace.PrintStates(lookup, 1);
            trace.PrintSteps(lookup, 1);

            // TODO Jonas: print dungeon trace graph
            // TODO Jonas: print item state trace graph
            // TODO Jonas: analyze player choices (branching traces)
            // TODO Jonas: analyze backtracking (revisiting locations with new items)
            // TODO Jonas: is traces merging interesting for players?
            // TODO Jonas: how to measure foreshadowing?
        }

        private static void PrintMission(Crawler crawler, VariableLookup lookup, int dungeonId)
        {
            using (var writer = File.CreateText($"dungeon{dungeonId}-mission.dot"))
            {
                var dot = new DotBuilder(writer);
                crawler.Express(dot);
            }
        }
    }
}
