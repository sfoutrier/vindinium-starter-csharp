using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace vindinium
{
    class FleeBot
    {
        private ServerStuff serverStuff;

        public FleeBot(ServerStuff serverStuff)
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

            while (serverStuff.finished == false && serverStuff.errored == false)
            {
                var stupidity = new ArtificialStupidity(serverStuff.myHero, serverStuff.heroes, serverStuff.board);
                Road road;
                int score = stupidity.PreferedRoad(out road);

                Console.WriteLine(serverStuff.myHero);
                if (road != null)
                {
                    var defaultColor = Console.ForegroundColor;
                    Console.Write("Aiming to ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(road.Target);
                    Console.ForegroundColor = defaultColor;
                    Console.Write(road.Destination + " " + road.Next + " distance of " + road.Length + " score of ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(score);
                    Console.ForegroundColor = defaultColor;

                    Console.WriteLine(road);
                    serverStuff.moveHero(road.Next);
                }
                else
                {
                    Console.WriteLine("Nowhere to go");
                    serverStuff.moveHero(Direction.Stay);
                }

                Console.WriteLine("completed turn " + serverStuff.currentTurn);
            }

            if (serverStuff.errored)
            {
                Console.WriteLine("error: " + serverStuff.errorText);
            }

            Console.WriteLine("random bot finished");
        }

        public class ArtificialStupidity
        {
            internal static readonly Tile[] TARGETS = new Tile[]
            {
                Tile.GOLD_MINE_NEUTRAL,
                Tile.GOLD_MINE_1,
                Tile.GOLD_MINE_2,
                Tile.GOLD_MINE_3,
                Tile.GOLD_MINE_4,
                Tile.HERO_1,
                Tile.HERO_2,
                Tile.HERO_3,
                Tile.HERO_4,
                Tile.TAVERN,
                Tile.FREE,
            };
            private const int LIFE_LOST_IN_MINE = 20;

            private Hero _myHero;
            private IList<Hero> _heroes;
            private Tile[][] _board;


            public ArtificialStupidity(Hero myHero, IList<Hero> heroes, Tile[][] board)
            {
                _myHero = myHero;
                _heroes = heroes;
                _board = board;
            }

            public int PreferedRoad(out Road chosenRoad)
            {
                int maxScore = int.MinValue;
                chosenRoad = null;
                foreach(var road in Road.AllPossibleRoads(_board, _myHero.pos.Revert))
                {
                    int roadScore = ScorePath(road);
                    if(roadScore > maxScore)
                    {
                        maxScore = roadScore;
                        chosenRoad = road;
                    }
                }

                return maxScore;
            }

            /// <summary>
            /// This is the AI core, I score roads to get the most relevant one
            /// </summary>
            /// <param name="road"></param>
            /// <returns></returns>
            public int ScorePath(Road road)
            {
                switch (road.Target)
                {
                    case Tile.IMPASSABLE_WOOD:
                        return int.MinValue;
                    case Tile.FREE:
                        return ScoreRoadToFree(road.Length);
                    case Tile.HERO_1:
                    case Tile.HERO_2:
                    case Tile.HERO_3:
                    case Tile.HERO_4:
                        return ScoreRoadToHero(HeroFromTile(road.Target), road.Length);
                    case Tile.TAVERN:
                        return ScoreRoadToTavern(road.Length);
                    case Tile.GOLD_MINE_NEUTRAL:
                    case Tile.GOLD_MINE_1:
                    case Tile.GOLD_MINE_2:
                    case Tile.GOLD_MINE_3:
                    case Tile.GOLD_MINE_4:
                        return ScoreRoadToMine(road.Target, road.Length);
                    default:
                        throw new Exception("Unkown target tile");
                }
            }

            public Hero HeroFromTile(Tile heroTile)
            {
                foreach (var hero in _heroes)
                    if (hero.HeroTile == heroTile)
                        return hero;
                throw new Exception("The tile does not match an hero");
            }

            /// <summary>
            /// Only use distance and life difference
            /// TODO : add a factor for the best score
            /// </summary>
            /// <param name="hero"></param>
            /// <param name="roadLength"></param>
            /// <returns></returns>
            public int ScoreRoadToHero(Hero hero, int roadLength)
            {
                if (hero.id == _myHero.id)
                    return ScoreRoadToFree(roadLength);

                // simple scoring
                // delta life
                // distance amplify
                var baseScore = _myHero.life - hero.life;
                return baseScore / roadLength;
            }

            /// <summary>
            /// Just takes the nearst mine
            /// score depends on life
            /// TODO : prefer stil mines from best ennemy
            /// TODO : avoid pathes where ennemy are
            /// </summary>
            /// <param name="mine"></param>
            /// <param name="roadLength"></param>
            /// <returns></returns>
            public int ScoreRoadToMine(Tile mine, int roadLength)
            {
                if (_myHero.MineTile == mine)
                    return 0;

                var lifeAtArrival = _myHero.life - roadLength - LIFE_LOST_IN_MINE;
                return lifeAtArrival;
            }

            /// <summary>
            /// Only use life and distance
            /// TODO : use the ennemy positions to avoid confronting them when HP is really needed
            /// </summary>
            /// <param name="roadLength"></param>
            /// <returns></returns>
            public int ScoreRoadToTavern(int roadLength)
            {
                return 100 - _myHero.life - roadLength;
            }

            /// <summary>
            /// Only use the distance to the target
            /// TODO : use ennemy proximity to better score it
            /// </summary>
            /// <param name="roadLength"></param>
            /// <returns></returns>
            public int ScoreRoadToFree(int roadLength)
            {
                return -roadLength;
            }
        }
    }
}
