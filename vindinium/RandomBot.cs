using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace vindinium
{
    class RandomBot
    {
        private ServerStuff serverStuff;

        public RandomBot(ServerStuff serverStuff)
        {
            this.serverStuff = serverStuff;
        }

        //starts everything
        public void run()
        {
            Console.Out.WriteLine("random bot running");

            serverStuff.createGame();

            if (serverStuff.errored == false)
            {
                //opens up a webpage so you can view the game, doing it async so we dont time out
                System.Diagnostics.Process.Start(serverStuff.viewURL);
            }

            Tile[] mines = LoadUsefullMines(serverStuff.myHero.id);

            while (serverStuff.finished == false && serverStuff.errored == false)
            {
                IList<Tile> targets;
                if (serverStuff.myHero.life > 50)
                    targets = new List<Tile>(mines);
                else if (serverStuff.myHero.life > 20)
                {
                    targets = new List<Tile>(mines);
                    targets.Add(Tile.TAVERN);
                }
                else
                    targets = new Tile[] { Tile.TAVERN };
                var heroPos = new Pos {x = serverStuff.myHero.pos.y, y = serverStuff.myHero.pos.x};
                var road = Road.ShortestRoadTo(heroPos, targets, serverStuff.board);

                Console.WriteLine(serverStuff.myHero);
                if (road != null)
                {
                    Console.WriteLine("Aiming to " + road.Target + road.Destination + " " + road.Next + " distance of " + road.Length);
                    Console.WriteLine(road);
                    serverStuff.moveHero(road.Next);
                }
                else
                {
                    Console.WriteLine("Nowhere to go");
                    serverStuff.moveHero(Direction.Stay);
                }

                Console.Out.WriteLine("completed turn " + serverStuff.currentTurn);
            }

            if (serverStuff.errored)
            {
                Console.Out.WriteLine("error: " + serverStuff.errorText);
            }

            Console.Out.WriteLine("random bot finished");
        }

        public static Tile[] LoadUsefullMines(int playerId)
        {
            Tile[] mines;
            switch (playerId)
            {
                case 1:
                    mines = new Tile[] { Tile.GOLD_MINE_NEUTRAL, Tile.GOLD_MINE_2, Tile.GOLD_MINE_3, Tile.GOLD_MINE_4 };
                    break;
                case 2:
                    mines = new Tile[] { Tile.GOLD_MINE_NEUTRAL, Tile.GOLD_MINE_1, Tile.GOLD_MINE_3, Tile.GOLD_MINE_4 };
                    break;
                case 3:
                    mines = new Tile[] { Tile.GOLD_MINE_NEUTRAL, Tile.GOLD_MINE_1, Tile.GOLD_MINE_2, Tile.GOLD_MINE_4 };
                    break;
                case 4:
                    mines = new Tile[] { Tile.GOLD_MINE_NEUTRAL, Tile.GOLD_MINE_1, Tile.GOLD_MINE_2, Tile.GOLD_MINE_3 };
                    break;
                default:
                    throw new Exception();
            }
            return mines;
        }
    }
}
