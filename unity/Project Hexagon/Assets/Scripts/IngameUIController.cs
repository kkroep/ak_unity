using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIController : MonoBehaviour
{

    // Global variables
    protected GameObject gameController;
    protected Component boardController;

    // Use this for initialization
    void Start()
    {
        // Get the gameController
        gameController = GameObject.FindGameObjectWithTag("GameController");
        boardController = gameController.GetComponent<BoardController>();

        // Initiate the champions for the player on the positions that are wished

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void selectChampion(int championNumber)
    {
        if (championNumber == 1)
        {

        }

        if (championNumber == 2)
        {

        }

        if (championNumber == 3)
        {

        }
    }
}
