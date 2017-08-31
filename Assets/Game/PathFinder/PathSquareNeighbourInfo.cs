
public class PathSquareNeighbourInfo
{
    public bool isDiagonal = false;
    public PathSquare[] diagonalCheck;

    public PathSquare neighbour;
    public double distance;

    public PathSquareNeighbourInfo(PathSquare neighbour, double dist)
    {
        this.neighbour = neighbour;
        this.isDiagonal = false;
        this.distance = dist;
    }

    public PathSquareNeighbourInfo(PathSquare neighbour, double dist, PathSquare[] check)
    {
        this.neighbour = neighbour;
        this.distance = dist;
        this.diagonalCheck = check;
        this.isDiagonal = true;
    }
}
