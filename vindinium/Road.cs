using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vindinium
{
    class Road
    {
        public int Length { get { return _internalRoad.Count - 1; } }
        public string Next 
        {
            get
            {
                if (Length > 0)
                {
                    var cur = _internalRoad[0];
                    var next = _internalRoad[1];

                    if (cur.x < next.x)
                        return Direction.East;
                    else if (cur.x > next.x)
                        return Direction.West;
                    else if (cur.y < next.y)
                        return Direction.South;
                    else if (cur.y > next.y)
                        return Direction.North;
                }
                return Direction.Stay;
            }
        }

        public Pos Destination { get { return _internalRoad.Last(); } }
        public Tile Target { get; private set; }

        internal List<Pos> _internalRoad = new List<Pos>();

        private Road()
        {
        }

        private Road(Road other)
        {
            this._internalRoad = new List<Pos>(other._internalRoad);
        }

        public static Road ShortestRoadTo(Pos start, IList<Tile> targets, Tile[][] map)
        {
            Road minRoad = null;

            for (int i = 0; i < map.Length; i++)
                for (int j = 0; j < map[i].Length; j++)
                    if(targets.Contains(map[i][j]))
                    {
                        var road = RoadTo(start, new Pos { x = i, y = j }, map);
                        if (road != null)
                            if (minRoad == null || minRoad.Length > road.Length)
                                minRoad = road;
                    }

            return minRoad;
        }

        public static Road RoadTo(Pos start, Pos target, Tile[][] map)
        {
            var targetTile = map[target.x][target.y];

            var workingMap = new Tile[map.Length][];
            for (int i = 0; i < map.Length; i++)
            {
                workingMap[i] = new Tile[map[i].Length];
                Array.Copy(map[i], workingMap[i], map[i].Length);
            }

            var possibleRoads = new LinkedList<Road>();
            var firstRoad = new Road();
            firstRoad._internalRoad.Add(start);
            possibleRoads.AddFirst(firstRoad);

            while(true)
            {
                var seed = ExtractShortestRoad(possibleRoads);
                if (seed == null)
                    return null;
                foreach (var road in RoadsFrom(seed, workingMap, target))
                {
                    var last = road._internalRoad.Last();
                    if (last.Equals(target))
                    {
                        road.Target = targetTile;
                        return road;
                    }
                    possibleRoads.AddLast(road);
                }
            }
        }

        static IEnumerable<Road> RoadsFrom(Road seed, Tile[][] map, Pos target)
        {
            var pos = seed._internalRoad.Last();
            Road nextRoad;
            if (TryMoveTo(seed, map, target, pos.x + 1, pos.y, out nextRoad))
                yield return nextRoad;
            if (TryMoveTo(seed, map, target, pos.x - 1, pos.y, out nextRoad))
                yield return nextRoad;
            if (TryMoveTo(seed, map, target, pos.x, pos.y + 1, out nextRoad))
                yield return nextRoad;
            if (TryMoveTo(seed, map, target, pos.x, pos.y - 1, out nextRoad))
                yield return nextRoad;
        }

        static bool TryMoveTo(Road seed, Tile[][] map, Pos target, int x, int y, out Road newRoad)
        {
            if (x >= 0 && y >= 0 && x < map.Length && y < map[x].Length 
                && (map[x][y] == Tile.FREE || (target.x == x && target.y == y)))
            {
                newRoad = new Road(seed);
                newRoad._internalRoad.Add(new Pos { x = x, y = y});
                map[x][y] = Tile.IMPASSABLE_WOOD;
                return true;
            }
            newRoad = null;
            return false;
        }

        static Road ExtractShortestRoad(LinkedList<Road> roads)
        {
            var current = roads.First;
            if (current == null)
                return null;

            var minLen = roads.Min(r => r.Length);
            while (current.Value.Length != minLen)
                current = current.Next;

            roads.Remove(current);
            return current.Value;
        }

        public override string ToString()
        {
            var bld = new StringBuilder();
            foreach (var pos in _internalRoad)
                bld.Append(pos);
            return bld.ToString();
        }
    }
}
