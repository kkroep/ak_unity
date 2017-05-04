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
}