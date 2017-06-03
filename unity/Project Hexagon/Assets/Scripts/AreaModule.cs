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
    private int[,] tileProperties;
    private int[] loc = new int[2];
    private int team;
    private int turn;
    private bool just_created;

    // Global variables
    private GameObject gameController;
    
    // Use this for initialization
    void Start() {
        loc = GetComponent<UnitController>().getCurrentPosition();
        gameController = GameObject.FindGameObjectWithTag("GameController");
        team = GetComponent<UnitController>().getPlayerID();
        turn = 0;
        just_created = true;

        tileProperties = gameController.GetComponent<BoardController>().tileProperties;

        // initialize FoV corretly
        Update_FoV(GetComponent<UnitController>().getCurrentPosition()[0], GetComponent<UnitController>().getCurrentPosition()[1]);
    }
	
    // Update is called once per frame
    void Update () {
		
    }

    public void Update_Territorium(int x, int y) {
        setTerritorium(x, y, 1);
        setTerritorium(x, y+1, 0);
        setTerritorium(x+1, y, 0);
        setTerritorium(x+1, y-1, 0);
        setTerritorium(x, y-1, 0);
        setTerritorium(x-1, y, 0);
        setTerritorium(x-1, y+1, 0);
        return;
    }

    // Updates field of view caused by this character
    public void Update_FoV(int x, int y) {
        turn++;
        if (loc[0] == x && loc[1] == y && just_created != true)
            return;
        just_created = false;

        //Debug.Log("moving from ("  + loc[0] + "," + loc[1] + ") to (" + x + "," + y+")");

        

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

        // remove everything that is obstructed by field of view


        /* Table of content tileProperties
         * 0 = empty
         * 1 = normal
         * 2 = mountain
         * 3 = forrest
         */
        for (int i = 1; i < 8; i++)
        {
            for (int j = 1; j < 8; j++)
            {
                if (x + i - 4 >= 0 && x + i - 4 < 20 && y + j - 4 >= 0 && y + j - 4 < 20)
                {
                    if (tileProperties[x + i - 4, y + j - 4] == 0)
                    {
                        new_FoV[i, j] = 0;
                    }
                }
            }
        }



        // report all differences that are found

        for (int i = 1; i < 8; i++) {
            for (int j = 1; j < 8; j++)
            {
                //Debug.Log(current_FoV[i,j]);
                //Debug.Log("moved (" + moved[0]+","+moved[1]+")");
                if (new_FoV[i, j] > current_FoV[i + moved[0], j + moved[1]] && x + i >= 4 && x+i-4 < 20 && y + j >= 4 && y+j-4 < 20)
                //if (new_FoV[i,j]>0 && x+i >= 4 && y+j >= 4)
                {
                    //Debug.Log("add_FoV (" + (x + i - 4) + "," + (y + j - 4) + ")");
                    gameController.GetComponent<BoardController>().getTile(x + i - 4, y + j - 4).GetComponent<HexagonScript>().setFoV(team);
                }
                if (current_FoV[i,j]>new_FoV[i-moved[0],j-moved[1]] && x+i-4-moved[0] >= 0 && x+i-4-moved[0] < 20 && y+j-4-moved[1] >= 0 && y+j-4-moved[1] < 20)
                //if (new_FoV[i,j]>0 && x+i >= 4 && y+j >= 4)
                {
                    //Debug.Log("del_FoV (" + (x+i-4-moved[0]) + "," + (y+j-4-moved[1]) + ")");
                    gameController.GetComponent<BoardController>().getTile(x + i - 4-moved[0], y + j - 4- moved[1]).GetComponent<HexagonScript>().setNoFoV(team);
                }
            }
        }

        current_FoV = new_FoV;

        //Update current location
        loc[0] = x;
        loc[1] = y;
    }
    
    public void setNoFoV(int x, int y) {
        if(x>=0 && y >=0 && x<20 && y<20)
            gameController.GetComponent<BoardController>().getTile(x, y).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        return;
    }

    public void setFoV(int x, int y) {
        if(x>=0 && y >=0 && x<20 && y<20)
            gameController.GetComponent<BoardController>().getTile(x, y).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        return;
    }

    public void setTerritorium(int x, int y, int force) {
        if(x>=0 && y >=0 && x<20 && y<20)
            gameController.GetComponent<BoardController>().getTile(x, y).GetComponent<HexagonScript>().claimTerritorium(turn, team, force);
        return;
    }
}
