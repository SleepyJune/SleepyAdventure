using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IPosition : IEquatable<IPosition>
{
    public int x;
    public int y;
    public int z;

    public IPosition(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static bool operator ==(IPosition value1, IPosition value2)
    {
        return value1.x == value2.x
                && value1.y == value2.y
                && value1.z == value2.z;
    }

    public static bool operator !=(IPosition value1, IPosition value2)
    {
        return !(value1 == value2);
    }

    public override bool Equals(object obj)
    {
        return (obj is IPosition) ? this == (IPosition)obj : false;
    }

    public bool Equals(IPosition other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return this.x * 100 + this.y * 10 + this.z;
    }

    public double DistanceSqr(IPosition pos2)
    {
        return ((pos2.x - this.x) * (pos2.x - this.x) + (pos2.y - this.y) * (pos2.y - this.y));
    }

    public double Distance(IPosition pos2)
    {
        return Math.Sqrt((pos2.x - this.x) * (pos2.x - this.x) + (pos2.y - this.y) * (pos2.y - this.y));
    }

    public IPosition To2D()
    {
        return new IPosition(x, 0, z);
    }

    public Vector3 ToVector()
    {
        return new Vector3(x, y/2.0f, z);
    }
}

public static class PositionExtensions
{
    public static IPosition ConvertToIPosition(this Vector3 vPos)
    {
        return new IPosition(
            (int)Mathf.Round(vPos.x), 
            (int)Mathf.Round(vPos.y), 
            (int)Mathf.Round(vPos.z));
    }
}
