using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vindinium
{
    class Road
    {
        public int Length { get { return _internalRoad.Count; } }
        public string Next 
        {
            get
            {
                if (Length >= 2)
                {
                    var cur = _internalRoad[0];
                    var next = _internalRoad[1];

                    if (cur.x < next.x)
                        return Direction.South;
                    else if (cur.x > next.x)
                        return Direction.North;
                    else if (cur.y < next.y)
                        return Direction.West;
                    else if (cur.y > next.y)
                        return Direction.East;
                }
                return Direction.Stay;
            }
        }

        internal List<Pos> _internalRoad = new List<Pos>();

        private Road()
        {
        }

        private Road(Road other)
        {
            this._internalRoad = new List<Pos>(other._internalRoad);
        }

        public static Road RoadTo(Pos start, Pos target, Tile[][] map)
        {
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

            //var shortestRoad = ExtractShortestRoad(possibleRoads);

            return null;
        }

        public Road ExtractShortestRoad(LinkedList<Road> roads)
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
    }
}
