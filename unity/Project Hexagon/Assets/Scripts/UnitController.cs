using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains the controlls for the unit itself, the position of the unit, and the future positions calculated will be stored in here:
/// 1. User feedback when unit is selected
/// 2. User feedback when unit is moving, and adjusting it's positions in the matrix of the GameController
/// 3. Calculating it's own movement path and saving it's path in it's own matrix with steps
/// 
/// ----TODO:
/// 
/// 1. Implement the Dijkstra algorithm, and make the unit save all the steps in a vector.
/// 2. Put the walking in a seperate function, that needs to be called by the Board Controller when the unit needs to walk.
/// </summary>

public class UnitController : MonoBehaviour
{
    // Unit information
    private int x, y; // Current coordinates of a certain unit


    // External set information
    public Material materialSelected;
    public Material materialNotSelected;
    
    // Global variables
    private GameObject gameController;
    private GameObject currentTile;

    void Start ()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    public void IsSelected()
    {
        // Feedback to user that he selected this unit
        currentTile = gameController.GetComponent<BoardController>().getTile(new Vector2(x, y)); // Get the tile
        currentTile.GetComponent<MeshRenderer>().material = materialSelected;
    }

    public void MoveUnit(int x_new, int y_new)
    {
        int old_x = x;
        int old_y = y;

        // Removing the selection visual feedback
        currentTile = gameController.GetComponent<BoardController>().getTile(new Vector2(old_x, old_y)); // Get the tile
        currentTile.GetComponent<MeshRenderer>().material = materialNotSelected; // Change the material of the tile

        // Move the unit to the new tile
        gameController.GetComponent<BoardController>().deleteUnit(old_x, old_y); // Removes the unit from the matrix (not the gameobject itself!!)
        gameController.GetComponent<BoardController>().setUnit(x_new, y_new, gameObject); // Sets THIS unit to new position in the matrix
        x = x_new; // Set new unit coordinates
        y = y_new;
        transform.position = gameController.GetComponent<BoardController>().getTile(x_new, y_new).transform.position + new Vector3(0, transform.position.y, 0); // Place the unit to the new tile position
    }

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
