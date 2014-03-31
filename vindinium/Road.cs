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

        public static Road ShortestRoadTo(Tile[][] map, Pos start, IList<Tile> targets)
        {
            Road minRoad = null;

            foreach (var target in targets)
            {
                var road = ShortestRoadTo(map, start, target);
                if (road != null)
                    if (minRoad == null || minRoad.Length > road.Length)
                        minRoad = road;
            }

            return minRoad;
        }

        public static Road ShortestRoadTo(Tile[][] map, Pos start, Tile target)
        {
            Road minRoad = null;

            for (int i = 0; i < map.Length; i++)
                for (int j = 0; j < map[i].Length; j++)
                    if(target == map[i][j])
                    {
                        var road = RoadTo(map ,start, new Pos { x = i, y = j });
                        if (road != null)
                            if (minRoad == null || minRoad.Length > road.Length)
                                minRoad = road;
                    }

            return minRoad;
        }

        public static Road RoadTo(Tile[][] map, Pos start, Pos target)
        {
            var targetTile = map[target.x][target.y];

            var workingMap = CopyBoard(map);

            var possibleRoads = new Queue<Road>();
            var firstRoad = new Road();
            firstRoad._internalRoad.Add(start);
            possibleRoads.Enqueue(firstRoad);

            while (possibleRoads.Count != 0)
            {
                var seed = possibleRoads.Dequeue();
                foreach (var road in RoadsFrom(seed, workingMap))
                {
                    var last = road._internalRoad.Last();
                    if (last.Equals(target))
                    {
                        road.Target = targetTile;
                        return road;
                    }
                    if(road.Target == Tile.FREE)
                        possibleRoads.Enqueue(road);
                }
            }

            return null;
        }

        public static IList<Road> AllPossibleRoads(Tile[][] map, Pos start)
        {
            var workingMap = CopyBoard(map);

            var candidateRoads = new Queue<Road>();
            var possibleRoads = new List<Road>();
            var firstRoad = new Road();
            firstRoad._internalRoad.Add(start);
            candidateRoads.Enqueue(firstRoad);

            while (candidateRoads.Count != 0)
            {
                var seed = candidateRoads.Dequeue();
                possibleRoads.Add(seed);

                foreach (var road in RoadsFrom(seed, workingMap))
                {
                    var last = road._internalRoad.Last();
                    if (road.Target == Tile.FREE)
                        candidateRoads.Enqueue(road);
                    else if (road.Target != Tile.IMPASSABLE_WOOD)
                        possibleRoads.Add(road);
                }
            }

            return possibleRoads;
        }

        private static Tile[][] CopyBoard(Tile[][] map)
        {
            var workingMap = new Tile[map.Length][];
            for (int i = 0; i < map.Length; i++)
            {
                workingMap[i] = new Tile[map[i].Length];
                Array.Copy(map[i], workingMap[i], map[i].Length);
            }

            return workingMap;
        }

        static IEnumerable<Road> RoadsFrom(Road seed, Tile[][] map)
        {
            var pos = seed._internalRoad.Last();
            Road nextRoad;
            if (TryMoveTo(seed, map, pos.x + 1, pos.y, out nextRoad))
                yield return nextRoad;
            if (TryMoveTo(seed, map, pos.x - 1, pos.y, out nextRoad))
                yield return nextRoad;
            if (TryMoveTo(seed, map, pos.x, pos.y + 1, out nextRoad))
                yield return nextRoad;
            if (TryMoveTo(seed, map, pos.x, pos.y - 1, out nextRoad))
                yield return nextRoad;
        }

        static bool TryMoveTo(Road seed, Tile[][] map, int x, int y, out Road newRoad)
        {
            if (x >= 0 && y >= 0 && x < map.Length && y < map[x].Length )
            {
                newRoad = new Road(seed);
                newRoad._internalRoad.Add(new Pos { x = x, y = y});
                newRoad.Target = map[x][y];
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