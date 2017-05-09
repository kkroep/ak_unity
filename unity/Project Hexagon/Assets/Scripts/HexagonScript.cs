using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonScript : MonoBehaviour
{
    private int x, y;


    public void set(int new_x, int new_y)
    {
        x = new_x;
        y = new_y;
    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }
}
