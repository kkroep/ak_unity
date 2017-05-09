using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HexMath : MonoBehaviour
{

    public float matrix2HexX(float x)
    {
        return x * 1.5f;
    }

    public float matrix2HexY(float x, float y)
    {
        if (x % 2 == 0)
            return y * (float)Math.Sqrt(3);
        else
            return (y + 0.5f) * (float)Math.Sqrt(3);
    }

    public float hexDistance(float x1, float y1, float x2, float y2) {
        float x_diff = matrix2HexX(x1 - x2);
        float y_diff = matrix2HexY(x1, y1) - matrix2HexY(x2, y2);
        return Math.Sqrt(x_diff*x_diff+y_diff*y_diff);
    }
}