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
        [Test]
        public void TestRoad()
        {
            var road = Road.RoadTo(
                new Pos { x = 1, y = 1 },
                new Pos { x = 2, y = 2 },
                new Tile[][]
                    {
                        new Tile[] { Tile.FREE, Tile.FREE,Tile.FREE,Tile.FREE,Tile.FREE, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.HERO_1,Tile.FREE,Tile.FREE,Tile.FREE, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.FREE,Tile.TAVERN,Tile.FREE,Tile.FREE, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.FREE,Tile.FREE,Tile.FREE,Tile.FREE, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.FREE,Tile.FREE,Tile.FREE,Tile.FREE, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.FREE,Tile.FREE,Tile.FREE,Tile.FREE, Tile.FREE},
                    });

            Assert.IsNotNull(road, "No road found");
            Assert.AreEqual(2, road.Length, "The road lenght should be 2");
            Assert.IsTrue(road.Next == Direction.East || road.Next == Direction.South, "The first direction should be east or south");
        }

        [Test]
        public void TestRoad2()
        {
            var road = Road.ShortestRoadTo(
                new Pos { x = 1, y = 1 },
                new Tile[] { Tile.GOLD_MINE_NEUTRAL, Tile.GOLD_MINE_2, Tile.GOLD_MINE_3, Tile.GOLD_MINE_4 },
                new Tile[][]
                    {
                        new Tile[] { Tile.FREE, Tile.IMPASSABLE_WOOD,   Tile.GOLD_MINE_NEUTRAL, Tile.FREE,              Tile.FREE, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.HERO_1,            Tile.TAVERN,            Tile.IMPASSABLE_WOOD,   Tile.FREE, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.FREE,              Tile.IMPASSABLE_WOOD,   Tile.IMPASSABLE_WOOD,   Tile.FREE, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.IMPASSABLE_WOOD,   Tile.GOLD_MINE_3,       Tile.FREE,              Tile.GOLD_MINE_3, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.FREE,              Tile.FREE,              Tile.IMPASSABLE_WOOD,   Tile.FREE, Tile.FREE},
                        new Tile[] { Tile.FREE, Tile.FREE,              Tile.FREE,              Tile.FREE,              Tile.FREE, Tile.FREE},
                    });

            Assert.IsNotNull(road, "No road found");
            Assert.AreEqual(2, road.Length, "The road lenght should be 2");
            Assert.IsTrue(road.Next == Direction.North, "The first direction should be north");
        }

        [Test]
        public void TestRoad3()
        {
            // http://vindinium.org/u99j2jag
            var boardSize = 20;
            var boardTile = "##              ########              ########  ########################  ############  ##$-[]############[]$-##  ############          ########@4        ######"
                          + "  $-  ##          ####          ##  $-            ##      ####      ##              $-  ####@1##        ##  ####  $-          ##        ##    ##        ##      "
                          + "      ####    ############    ####              ########################                ########################              ####@2  ############    ####      "
                          + "      ##        ##    ##        ##          $-  ####  ##        ##  ####  $-              ##      ####      ##            $-  ##          ####          ##  $-  "
                          + "######          ########@3        ############  ##$-[]############[]$3##  ############  ########################  ########              ########              ##";
            var board = ServerStuff.createBoard(boardSize, boardTile);

            Pos hero1Pos = null;
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    if (board[i][j] == Tile.HERO_1)
                        hero1Pos = new Pos {x = i, y = j};

            var minesTile = RandomBot.LoadUsefullMines(1);

            var road = Road.ShortestRoadTo(hero1Pos, minesTile, board);
            Console.WriteLine(road);
        }
    }
}
