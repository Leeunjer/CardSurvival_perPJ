
using UnityEngine;

public struct CubeCoord
{
    public int x;
    public int y;
    public int z;
    public CubeCoord(int x,int y, int z){
        this.x = x;
        this.y = y;
        this.z = z;
    }

}

public interface IHoverable
{
    void OnClicked();
    void OnHoverEnter();
    void OnHoverExit();
}



public class Utils 
{
    public static CubeCoord OffsetToCube(Vector2Int offset)
    {
        int offsetX = offset.x - (offset.y + (offset.y & 1)) / 2;
        int offsetZ = offset.y;
        int offsetY = -offsetX - offsetZ;

        return new CubeCoord(offsetX, offsetY , offsetZ);
    }

    public static int GetHexDistance(Vector2Int a , Vector2Int b)
    {
        CubeCoord cubeA = OffsetToCube(a);
        CubeCoord cubeB = OffsetToCube(b);

        return Mathf.Max(Mathf.Abs(cubeA.x - cubeB.x), Mathf.Abs(cubeA.y - cubeB.y), Mathf.Abs(cubeA.z - cubeB.z));

    }
    
}

