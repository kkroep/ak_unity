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
    public Material team_1_mat;
    public Material no_team_mat;

    // Global variables
    private GameObject gameController;
    
    // Use this for initialization
    void Start() {
        loc = GetComponent<UnitController>().getCurrentPosition();
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }
	
    // Update is called once per frame
    void Update () {
		
    }

    // Updates field of view caused by this character
    public void Move_FoV(int x, int y) {
        if (loc[0] == x && loc[1] == y)
            return;

        Debug.Log("moving from ("  + loc[0] + "," + loc[1] + ") to (" + x + "," + y+")");

        

        int[] moved = new int[2];
        moved[0] = x - loc[0];
        moved[1] = y - loc[1];

        /* 
         * calculate a new field of view matrix and calculate the difference with the previous FoV matrix
         */

        int[,] new_FoV = new int[9, 9] {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 1, 1, 1, 1, 0},
            { 0, 0, 0, 1, 1, 1, 1, 1, 0},
            { 0, 0, 1, 1, 1, 1, 1, 1, 0},
            { 0, 1, 1, 1, 1, 1, 1, 1, 0},
            { 0, 1, 1, 1, 1, 1, 1, 0, 0},
            { 0, 1, 1, 1, 1, 1, 0, 0, 0},
            { 0, 1, 1, 1, 1, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };

        new_FoV = new int[9, 9] {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 1, 1, 0, 0, 0},
            { 0, 0, 0, 1, 1, 1, 0, 0, 0},
            { 0, 0, 0, 1, 1, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };
        
        // remove everything that is obstructed by field of view
        



        // report all differences that are found

        for (int i = 1; i < 8; i++) {
            for (int j = 1; j < 8; j++)
            {
                //Debug.Log(current_FoV[i,j]);
                //Debug.Log("moved (" + moved[0]+","+moved[1]+")");
                if (new_FoV[i, j] > current_FoV[i + moved[0], j + moved[1]] && x + i >= 4 && y + j >= 4)
                //if (new_FoV[i,j]>0 && x+i >= 4 && y+j >= 4)
                {
                    Debug.Log("add_FoV (" + (x + i - 4) + "," + (y + j - 4) + ")");
                    setTerritorium(x + i - 4, y + j - 4);
                }
                if (current_FoV[i,j]>new_FoV[i-moved[0],j-moved[1]] && x+i-moved[0] >= 4 && y+j-moved[1] >= 4)
                //if (new_FoV[i,j]>0 && x+i >= 4 && y+j >= 4)
                {
                    Debug.Log("del_FoV (" + (x+i-4-moved[0]) + "," + (y+j-4-moved[1]) + ")");
                    setNoTerritorium(x + i - 4-moved[0], y + j - 4- moved[1]);
                }
            }
        }

        current_FoV = new_FoV;

        



        //Update current location
        loc[0] = x;
        loc[1] = y;
    }
    
    public void setNoTerritorium(int x, int y) {
        gameController.GetComponent<BoardController>().getTile(x, y).GetComponent<Renderer>().material = no_team_mat;
        return;
    }

    public void setTerritorium(int x, int y) {
        gameController.GetComponent<BoardController>().getTile(x, y).GetComponent<Renderer>().material = team_1_mat;
        return;
    }
}
