using System;
using UnityEngine;

/// <summary>
/// CircleMath class store values for for unit circle. This class function as a circular array.
/// </summary>
public class CircleMath
{
    /// <summary>
    /// Count of point on unit circle. This value must be a multiple of 4.
    /// </summary>
    private int vertexCount = 16;
    private Vector2[] circleMath;

    /// <summary>
    /// Index of point on unit circle.
    /// </summary>
    private int currentIndex = 0;

    public CircleMath(int vertexCount)
    {
        this.vertexCount = vertexCount;
        circleMath = new Vector2[vertexCount];
        Compute();
    }

    /// <summary>
    /// Computes a vertexCount amount of points on unit circle.
    /// </summary>
    private void Compute()
    {
        float angleStep = 2 * Mathf.PI / vertexCount;
        float angle;

        for (int i = 0; i < circleMath.Length; i++)
        {
            angle = angleStep * i;
            circleMath[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }

    /// <summary>
    /// Set current index to beginning so we can start iterate from left of a circle.
    /// </summary>
    public void StartFromZero()
    {
        currentIndex = 0;
    }

    /// <summary>
    /// Set current index to 1/4 of vertexCount so we can start iterate from top of a circle.
    /// </summary>
    public void StartFromNorth()
    {
        currentIndex = vertexCount / 4;
    }

    /// <summary>
    /// Set current index to half of vertexCount so we can start iterate from half of a circle.
    /// </summary>
    public void StartFromWest()
    {
        currentIndex = vertexCount / 2;
    }

    /// <summary>
    /// Set current index to 3/4 of vertexCount so we can start iterate from bottom of a circle.
    /// </summary>
    public void StartFromSouth()
    {
        currentIndex = (3 * vertexCount) / 4;
    }

    /// <summary>
    /// Get current point on unit circle.
    /// </summary>
    public Vector2 Current()
    {
        if (circleMath.Length <= currentIndex)
        {
            currentIndex = 0;
        }

        return circleMath[currentIndex++];
    }
}
