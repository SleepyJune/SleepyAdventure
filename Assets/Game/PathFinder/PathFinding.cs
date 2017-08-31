using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System.Linq;

public class Pathfinding
{

    public static Dictionary<IPosition, PathSquare> pathSquares = new Dictionary<IPosition, PathSquare>();

    public static void InitPathSquares(Level level)
    {
        pathSquares = new Dictionary<IPosition, PathSquare>();

        List<IPosition> neighbourPositions = new List<IPosition>()
        {
            new IPosition(-1, 0, 0), //4, left            
            new IPosition(1, 0, 0), //6, right
            new IPosition(0, 0, -1), //8, up
            new IPosition(0, 0, 1), //2, down            
        };

        Dictionary<IPosition, IPosition[]> neighbourDiagonalPositions = new Dictionary<IPosition, IPosition[]>()
        {
            { new IPosition(-1, 0, 1),
                new IPosition[2]{
                    new IPosition(-1, 0, 0), //4, left
                    new IPosition(0, 0, 1), //2, down
                } },//1
            { new IPosition(-1, 0, -1), new IPosition[2]{
                    new IPosition(-1, 0, 0), //4, left
                    new IPosition(0, 0, -1), //8, up
                } },//7
            { new IPosition(1, 0, -1), new IPosition[2]{
                    new IPosition(0, 0, -1), //8, up
                    new IPosition(1, 0, 0), //6, right
                } },//9
            { new IPosition(1, 0, 1), new IPosition[2]{
                    new IPosition(1, 0, 0), //6, right
                    new IPosition(0, 0, 1), //2, down
                } },//3     
        };

        

        foreach (var sqr in level.map.Values)
        {
            var newPathSqr = new PathSquare(sqr);

            pathSquares.Add(sqr.position, newPathSqr);

            sqr.CheckWalkable();
        }

        foreach (var pathSqr in pathSquares.Values)
        {
            var sqrPosition = pathSqr.pos;

            foreach (var dir in neighbourPositions)
            {
                var pos = sqrPosition + dir;

                PathSquare square;
                if (pathSquares.TryGetValue(pos, out square))
                {
                    pathSqr.neighbours.Add(square.pos, new PathSquareNeighbourInfo(square, square.Distance(pathSqr)));
                }
            }

            foreach (var diagonal in neighbourDiagonalPositions)
            {
                var pos = sqrPosition + diagonal.Key;

                PathSquare square;
                if (pathSquares.TryGetValue(pos, out square))
                {
                    var diagPos1 = sqrPosition + diagonal.Value[0];
                    var diagPos2 = sqrPosition + diagonal.Value[1];

                    PathSquare sqr1;
                    PathSquare sqr2;

                    if(pathSquares.TryGetValue(diagPos1, out sqr1) 
                        && pathSquares.TryGetValue(diagPos2, out sqr2))
                    {
                        pathSqr.neighbours.Add(square.pos, 
                            new PathSquareNeighbourInfo(square, square.Distance(pathSqr), 
                            new PathSquare[] { sqr1, sqr2}));
                    }
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

        if (pathSquares.TryGetValue(ipos, out sqr))
        {
            return sqr;
        }
        else
        {
            return null;
        }
    }

    public static PathInfo GetShortestPath(Unit unit, Vector3 start, Vector3 end)
    {
        PathSquare startSqr = GetPathSquare(start);
        PathSquare endSqr = GetPathSquare(end);

        //Debug.Log(end.x);

        if (start != null && end != null)
        {
            return GetShortestPath(unit, startSqr, endSqr);
        }

        return null;
    }

    public static bool CanWalkToSquare(Unit unit, PathSquareNeighbourInfo neighbourInfo, bool checkUnitCollision = true)
    {
        if(neighbourInfo.isDiagonal == false)
        {
            return CanWalkToSquare(unit, neighbourInfo.neighbour, checkUnitCollision);
        }
        else
        {
            return CanWalkToSquare(unit, neighbourInfo.neighbour, checkUnitCollision)
                && CanWalkToSquare(unit, neighbourInfo.diagonalCheck[0], checkUnitCollision)
                && CanWalkToSquare(unit, neighbourInfo.diagonalCheck[1], checkUnitCollision);
        }
    }

    public static bool CanWalkToSquare(Unit unit, PathSquare pathSquare, bool checkUnitCollision = false)
    {
        var sqr = pathSquare.square;
               
        if(sqr.obstacles.Count == 0)
        {
            return sqr.isWalkable;
        }
        else
        {
            if (checkUnitCollision)
            {
                foreach (var obj in sqr.obstacles.Values)
                {
                    if (obj != unit)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public static PathInfo GetShortestPath(Unit unit, PathSquare start, PathSquare end)
    {
        HashSet<PathSquare> closedSet = new HashSet<PathSquare>();
        HashSet<PathSquare> openSet = new HashSet<PathSquare>();

        PathSquare closestSquare = null;
        double closestDistance = 9999;

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
                var neighbourInfo = pair.Value;
                var neighbour = neighbourInfo.neighbour;
                var distance = neighbourInfo.distance;

                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                if (!CanWalkToSquare(unit, neighbourInfo))
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

                var estimatedDistance = neighbour.Distance(end);

                neighbour.parent = current;
                neighbour.gScore = alternativeDistance;
                neighbour.fScore = alternativeDistance + estimatedDistance;

                if(closestDistance > estimatedDistance)
                {
                    closestSquare = neighbour;
                    closestDistance = estimatedDistance;
                }

            }
        }

        if(closestSquare == null)
        {
            return null;
        }

        if (closestSquare.Distance(end) < start.Distance(end))
        {
            var path = PathInfo.GenerateWaypoints(start, closestSquare);
            path.reachable = false;

            return path;
        }
        else
        {
            return null;
        }
    }

    public static bool CanWalkToSquare(Unit unit, IPosition to)
    {
        PathSquare fromSqr;
        PathSquareNeighbourInfo toSqrInfo;
                
        if(pathSquares.TryGetValue(unit.pos2d, out fromSqr))
        {
            if(fromSqr.neighbours.TryGetValue(to, out toSqrInfo))
            {                
                return CanWalkToSquare(unit, toSqrInfo, true);
            }
        }

        return false;
    }

}
