using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
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

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
