using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Terrain lookup table
 * 0: void
 * 1: normal
 * 2: mountain
 */

/* This module is attached to characters in the game and is used to keep track of the visibility and the territorium 
 *
 */
public class AreaModule : MonoBehaviour {
    private int[,] current_FoV = new int[9,9];
    private int[,] terrain = new int[20,20];
    private int[] loc = new int[2];

	// Use this for initialization
	void Start() {
        //loc[0] = 4;
        //loc[1] = 5;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Updates field of view caused by this character
    public void Move_FoV(int x, int y) {
        Debug.Log("moving from ("  + loc[0] + "," + loc[1] + ") to (" + x + "," + y+")");

        int[] moved = new int[2];
        moved[0] = x - loc[0];
        moved[1] = x - loc[1];

        /* 
         * calculate a new field of view matrix and calculate the difference with the previous FoV matrix
         */

        int[,] new_FoV = new int[9, 9] {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 1, 1, 1, 1, 0, 0, 0, 0},
            { 0, 1, 1, 1, 1, 1, 0, 0, 0},
            { 0, 1, 1, 1, 1, 1, 1, 0, 0},
            { 0, 1, 1, 1, 1, 1, 1, 1, 0},
            { 0, 0, 1, 1, 1, 1, 1, 1, 0},
            { 0, 0, 0, 1, 1, 1, 1, 1, 0},
            { 0, 0, 0, 0, 1, 1, 1, 1, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };

        // remove everything that is obstructed by field of view

        // report all differences that are found

        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++)
            {
                if (new_FoV[i,j]!=current_FoV[i-moved[0],j-moved[1]])
                {
                    Debug.Log("(" + moved[0] + "," + moved[1]+ ")");
                }
            }
        }

        current_FoV = new_FoV;

        



        //Update current location
        loc[0] = x;
        loc[1] = y;
    }
    

}
