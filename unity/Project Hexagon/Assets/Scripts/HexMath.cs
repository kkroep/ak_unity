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
        return (y+0.5f*x) * (float)Math.Sqrt(3);
    }

    public float hexDistance(int x1, int y1, int x2, int y2) {
        float x_diff = matrix2HexX(x1 - x2);
        float y_diff = matrix2HexY(x1, y1) - matrix2HexY(x2, y2);
        return (float)Math.Sqrt(x_diff*x_diff+y_diff*y_diff);
    }
}