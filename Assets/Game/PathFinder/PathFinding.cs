using System.Collections;
using System.Collections.Generic;

using System.Linq;

public class PathFinding {

    public static Dictionary<IPosition, PathSquare> pathSquares = new Dictionary<IPosition, PathSquare>();

    public static void InitPathSquares(Level level)
    {
        List<IPosition> neighbourPositions = new List<IPosition>()
        {
            new IPosition(-1, 0, 0), //left            
            new IPosition(1, 0, 0), //right
            new IPosition(-1, 0, 0), //up
            new IPosition(1, 0, 0), //down

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
            foreach(var pos in neighbourPositions)
            {
                PathSquare square;
                if (pathSquares.TryGetValue(pos, out square))
                {
                    pathSqr.neighbours.Add(square);
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

    public static void GetShortestPath(PathSquare start, PathSquare end)
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
                //PrintPath(current);
                return;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbour in current.neighbours)
            {
                var distance = 1; //each square is 1 distance from each other

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
        
    }

}
