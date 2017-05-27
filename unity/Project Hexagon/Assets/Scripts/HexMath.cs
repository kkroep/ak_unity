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

    public float hexDistance(int x, int y) {
        float x_diff = matrix2HexX(x);
        float y_diff = matrix2HexY(x, y);
        return (float)Math.Sqrt(x_diff*x_diff+y_diff*y_diff);
    }
}