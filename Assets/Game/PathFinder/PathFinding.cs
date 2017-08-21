using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System.Linq;

public class Pathfinding {

    public static Dictionary<IPosition, PathSquare> pathSquares = new Dictionary<IPosition, PathSquare>();

    public static void InitPathSquares(Level level)
    {
        pathSquares = new Dictionary<IPosition, PathSquare>();

        List<IPosition> neighbourPositions = new List<IPosition>()
        {
            new IPosition(-1, 0, 0), //left            
            new IPosition(1, 0, 0), //right
            new IPosition(0, 0, -1), //up
            new IPosition(0, 0, 1), //down

            new IPosition(-1, 0, 1), //1
            new IPosition(-1, 0, -1), //7
            new IPosition(1, 0, -1), //9
            new IPosition(1, 0, 1), //3
        };

        foreach(var sqr in level.map.Values)
        {
            var newPathSqr = new PathSquare(sqr);

            pathSquares.Add(sqr.position, newPathSqr);
        }

        foreach(var pathSqr in pathSquares.Values)
        {
            var sqrPosition = pathSqr.pos;

            foreach(var dir in neighbourPositions)
            {
                var pos = sqrPosition + dir;

                PathSquare square;
                if (pathSquares.TryGetValue(pos, out square))
                {
                    pathSqr.neighbours.Add(square, square.Distance(pathSqr));
                }
            }
        }
    }

    public static void ResetPathSquare()
    {
        foreach (var sqr in pathSquares.Values)
        {
            sqr.gScore = 9999;
            sqr.fScore = 9999;
            sqr.parent = null;
        }
    }

    public static PathSquare GetPathSquare(Vector3 pos)
    {
        IPosition ipos = pos.ConvertToIPosition().To2D();
        PathSquare sqr;
        
        if(pathSquares.TryGetValue(ipos, out sqr))
        {
            return sqr;
        }
        else
        {
            return null;
        }
    }

    public static PathInfo GetShortestPath(Vector3 start, Vector3 end)
    {
        PathSquare startSqr = GetPathSquare(start);
        PathSquare endSqr = GetPathSquare(end);
                
        if(start != null && end != null)
        {
            return GetShortestPath(startSqr, endSqr);
        }

        return null;
    }

    public static PathInfo GetShortestPath(PathSquare start, PathSquare end)
    {
        HashSet<PathSquare> closedSet = new HashSet<PathSquare>();
        HashSet<PathSquare> openSet = new HashSet<PathSquare>();
        
        openSet.Add(start);

        ResetPathSquare();

        start.gScore = 0;
        start.fScore = start.Distance(end);

        while (openSet.Count > 0)
        {
            var current = openSet.OrderBy(n => n.fScore).FirstOrDefault();

            if (current == end)
            {
                return PathInfo.GenerateWaypoints(start, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var pair in current.neighbours)
            {
                var neighbour = pair.Key;
                var distance = pair.Value;
                
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                var alternativeDistance = current.gScore + distance;
                if (!openSet.Contains(neighbour))
                {
                    openSet.Add(neighbour);
                }
                else if (alternativeDistance >= neighbour.gScore)
                {
                    continue;
                }

                neighbour.parent = current;
                neighbour.gScore = alternativeDistance;
                neighbour.fScore = alternativeDistance + neighbour.Distance(end);

            }
        }

        return null;   
    }

}
