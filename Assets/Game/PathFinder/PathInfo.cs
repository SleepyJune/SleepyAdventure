using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class PathInfo
{
    public List<IPosition> waypoints;
    public List<IPosition> points;

    public IPosition start;
    public IPosition end;

    public bool reachable = true;

    public PathInfo(IPosition start, IPosition end)
    {
        this.start = start;
        this.end = end;

        waypoints = new List<IPosition>();
        points = new List<IPosition>();
    }

    public static PathInfo GenerateWaypoints(PathSquare start, PathSquare end)
    {
        var newPath = new PathInfo(start.pos, end.pos);

        var current = end;

        var lastSqr = end;
        Vector3 dir = Vector3.zero;
                
        while (current != null)
        {
            /*if(lastSqr != end)
            {
                var lastVec = lastSqr.pos.ToVector();
                var curVec = current.pos.ToVector();

                var tDir = (curVec - lastVec).normalized;

                Debug.Log(tDir);

                if(tDir != dir)
                {
                    newPath.waypoints.Add(current.pos);
                    dir = tDir;
                }
            }
            else
            {
                newPath.waypoints.Add(current.pos);
            }*/

            newPath.points.Add(current.pos);

            lastSqr = current;
            current = current.parent;
        }

        newPath.points.Reverse();
        newPath.waypoints.Reverse();

        return newPath;
    }
}