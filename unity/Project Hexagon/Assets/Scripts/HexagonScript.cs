using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonScript : MonoBehaviour
{
    private int x, y;

    private int turn_edited;
    private int current_team;
    private int forced;
    public Material team_0_mat;
    public Material team_1_mat;
    public Material no_team_mat;
    private GameObject FoVChild;

    int[] FoVCounter;

    public void Start()
    {
        FoVChild = gameObject.transform.GetChild(0).gameObject;
        FoVCounter = new int[2] { 0, 0 };
        return;
    }

    public void set(int new_x, int new_y)
    {
        x = new_x;
        y = new_y;
        turn_edited = -1;
        current_team = -1;
    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public void claimTerritorium(int turn, int team, int force)
    {
        if (current_team == team)
        {
            turn_edited = turn;
            forced = force;
            return;
        }
        if (forced > 0 && turn_edited == turn) {
            return;
        }
        if (turn == turn_edited && force == 0)
        {
            current_team = -1;
            GetComponent<Renderer>().material = no_team_mat;
            forced = force;
            return;
        }
        else
        {
            turn_edited = turn;
            current_team = team;
            forced = force;
            if(team==0)
                GetComponent<Renderer>().material = team_0_mat;
            else
                GetComponent<Renderer>().material = team_1_mat;
        }
        return;
    }

    public void setNoFoV(int team) {
        FoVCounter[team]--;
        if (FoVCounter[0] == 0 && FoVCounter[1] == 0 && current_team == -1)
            FoVChild.GetComponent<MeshRenderer>().enabled = true;
    }

    public void setFoV(int team) {
        FoVCounter[team]++;
        if (FoVCounter[0] == 1)
            FoVChild.GetComponent<MeshRenderer>().enabled = false;
    }



}
