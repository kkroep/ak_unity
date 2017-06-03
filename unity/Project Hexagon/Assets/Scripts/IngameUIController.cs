using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIController : MonoBehaviour
{

    // Global variables
    protected GameObject gameController;
    protected List<GameObject> unitList;

    // Use this for initialization
    void Start()
    {
        // Get the gameController
        gameController = GameObject.FindGameObjectWithTag("GameController");
        unitList = gameController.GetComponent<BoardController>().getUnitList(); // For now get the unit list that is in the BoardController

        // Initiate the champions for the player on the positions that are wished

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void selectChampion(int championNumber)
    {
        int teamOffset = -1;
        if (gameController.GetComponent<TileDetector>().getTeamID() == 1)
            teamOffset = 2;

        gameController.GetComponent<TileDetector>().selectPlayerUnit(unitList[championNumber+teamOffset]);
    }
}
