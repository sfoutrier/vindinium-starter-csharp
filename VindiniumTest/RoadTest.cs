using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using vindinium;

namespace VindiniumTest
{
    [TestFixture]
    public class RoadTest
    {
        public void TestRoad()
        {
            var road = Road.RoadTo(
                new Pos { x = 1, y = 1 },
                new Pos { x = 2, y = 2 },
                new Tile[][]
                    {
                        new Tile[] { Tile.FREE, Tile.FREE,Tile.FREE,Tile.FREE,Tile.FREE, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.FREE,Tile.FREE,Tile.FREE,Tile.FREE, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.FREE,Tile.FREE,Tile.FREE,Tile.FREE, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.FREE,Tile.FREE,Tile.FREE,Tile.FREE, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.FREE,Tile.FREE,Tile.FREE,Tile.FREE, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.FREE,Tile.FREE,Tile.FREE,Tile.FREE, Tile.FREE},
                    }
                );

            Assert.AreEqual(2, road.Length, "The road lenght should be 2");
            Assert.IsTrue(road.Next == Direction.East || road.Next == Direction.South, "The road lenght should be 2");
        }
    }
}
