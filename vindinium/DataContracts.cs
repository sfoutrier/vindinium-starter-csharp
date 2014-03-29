using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace vindinium
{
    [DataContract]
    class GameResponse
    {
        [DataMember]
        internal Game game;

        [DataMember]
        internal Hero hero;

        [DataMember]
        internal string token;

        [DataMember]
        internal string viewUrl;

        [DataMember]
        internal string playUrl;
    }

    [DataContract]
    class Game
    {
        [DataMember]
        internal string id;

        [DataMember]
        internal int turn;

        [DataMember]
        internal int maxTurns;

        [DataMember]
        internal List<Hero> heroes;

        [DataMember]
        internal Board board;

        [DataMember]
        internal bool finished;
    }

    [DataContract]
    class Hero
    {
        [DataMember]
        internal int id;

        [DataMember]
        internal string name;

        [DataMember]
        internal int elo;

        [DataMember]
        internal Pos pos;

        [DataMember]
        internal int life;

        [DataMember]
        internal int gold;

        [DataMember]
        internal int mineCount;

        [DataMember]
        internal Pos spawnPos;

        [DataMember]
        internal bool crashed;

        public Tile HeroTile 
        { 
            get 
            {
                switch (id)
                {
                    case 1:
                        return Tile.HERO_1;
                    case 2:
                        return Tile.HERO_2;
                    case 3:
                        return Tile.HERO_3;
                    case 4:
                        return Tile.HERO_4;
                    default:
                        throw new Exception();
                }
            }
        }

        public Tile MineTile
        {
            get
            {
                switch (id)
                {
                    case 1:
                        return Tile.GOLD_MINE_1;
                    case 2:
                        return Tile.GOLD_MINE_2;
                    case 3:
                        return Tile.GOLD_MINE_3;
                    case 4:
                        return Tile.GOLD_MINE_4;
                    default:
                        throw new Exception();
                }
            }
        }

        public override string ToString()
        {
            return name + " " + id + " " + pos + " gold:" + gold + " life:" + life; 
        }
    }

    [DataContract]
    class Pos
    {
        [DataMember]
        internal int x;

        [DataMember]
        internal int y;

        public Pos Revert
        {
            get
            {
                return new Pos { x = this.y, y = this.x };
            }
        }

        public override int GetHashCode()
        {
            return x ^ (y << 16);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Pos;
            return other.x == x && other.y == y;
        }

        public override string ToString()
        {
            return "{x=" + x + ";y=" + y + "}";
        }
    }

    [DataContract]
    class Board
    {
        [DataMember]
        internal int size;

        [DataMember]
        internal string tiles;
    }
}
