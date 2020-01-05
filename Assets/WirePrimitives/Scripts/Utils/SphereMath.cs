using UnityEngine;

/// <summary>
/// CircleMath class store values for for unit sphere.
/// </summary>
public class SphereMath
{
    /// <summary>
    /// Count of point on unit circle. This value must be a multiple of 4.
    /// </summary>
    private int yVertexCount = 16;
    private int zVertexCount = 16;
    private Vector3[,] sphereVertices;

    public int YVertexCount
    {
        get { return yVertexCount; }
    }

    public int ZVertexCount
    {
        get { return zVertexCount; }
    }

    public SphereMath(int yVertexCount, int zVertexCount)
    {
        this.yVertexCount = yVertexCount;
        this.zVertexCount = zVertexCount;
        sphereVertices = new Vector3[yVertexCount, zVertexCount];
        Compute();
    }

    /// <summary>
    /// Computes a vertexCount amount of points on unit sphere.
    /// </summary>
    private void Compute()
    {
        float yAngleStep = Mathf.PI / (yVertexCount - 1);//
        float zAngleStep = 2 * Mathf.PI / zVertexCount;
        float yAngle;
        float zAngle;

        for (int i = 0; i < sphereVertices.GetLength(0); i++)
        {
            yAngle = yAngleStep * i;
            for (int j = 0; j < sphereVertices.GetLength(1); j++)
            {
                zAngle = zAngleStep * j;
                sphereVertices[i, j] = new Vector3( Mathf.Cos(zAngle) * Mathf.Sin(yAngle),                                                    
                                                    Mathf.Cos(yAngle),
                                                    Mathf.Sin(zAngle) * Mathf.Sin(yAngle));
            }
        }
    }

    public Vector3 GetPoint(int yi, int zi)
    {
        return sphereVertices[yi, zi];
    }


}
