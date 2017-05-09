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
/// 1. Implement the Dijkstra algorithm, and make the unit save all the steps in a vector. -- DONE
/// 2. Put the walking in a seperate function, that needs to be called by the Board Controller when the unit needs to walk.
/// </summary>

public class UnitController : MonoBehaviour
{
    // Unit information
    private int x, y; // Current coordinates of a certain unit
    List<int[]> moveQueue = new List<int[]>(); // The list of places if it needs to move, else it's empty

    // External set information
    public Material materialSelected;
    public Material materialNotSelected;
    public GameObject selectedRing;
    
    // Global variables
    private GameObject gameController;
    private GameObject currentTile;
    private int[,] boardSize;

    void Start ()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        boardSize = new int[ gameController.GetComponent<BoardController>().boardsize[0], gameController.GetComponent<BoardController>().boardsize[1] ]; //TODO: Think about this huge matrix

        //Create the ring that determines the feedback, hide it when not selected
        createSelectionRing();
    }

    public void CalculatePath(int x_new, int y_new)
    {
        int[] oldCoordinates = new int[2] { x, y };
        int[] newCoordinates = new int[2] { x_new, y_new };

        // Removing the selection visual feedback
        removeSelectionFeedback();

        // Check if there has been a previous movement path, if yes, clear the feedback of it
        foreach (int[] location in moveQueue)
        {
            currentTile = gameController.GetComponent<BoardController>().getTile(new Vector2(location[0], location[1])); // Get the tile
            currentTile.GetComponent<MeshRenderer>().material = materialNotSelected;
        }

        // Check if player wants to cancel the move order by pressing the unit again
        if (x_new == x && y_new == y)
        {
            moveQueue.Clear(); // Clear the queue list
            return;
        }

        // Calculating the new movement path
        moveQueue = gameController.GetComponent<Dijkstra>().route(boardSize, oldCoordinates, newCoordinates);

        // Visual feedback for selected path
        foreach (int[] location in moveQueue)
        {
            currentTile = gameController.GetComponent<BoardController>().getTile(new Vector2(location[0], location[1])); // Get the tile
            currentTile.GetComponent<MeshRenderer>().material = materialSelected;
        }
    }

    // This function will move the unit by 1 step, each time it is called from the BoardController
    public void MoveUnit()
    {
        int[] oldCoordinates = { x, y };
        int[] newCoordinates = moveQueue[0];

        // Removing part of the path feedback
        removeSelectionFeedback();

        // Move the unit to the new tile
        gameController.GetComponent<BoardController>().deleteUnit(oldCoordinates[0], oldCoordinates[1]); // Removes the unit from the matrix (not the gameobject itself!!)
        gameController.GetComponent<BoardController>().setUnit(newCoordinates[0], newCoordinates[1], gameObject); // Sets THIS unit to new position in the matrix
        x = newCoordinates[0]; // Set new unit coordinates
        y = newCoordinates[1];
        transform.position = gameController.GetComponent<BoardController>().getTile(newCoordinates[0], newCoordinates[1]).transform.position + new Vector3(0, transform.position.y, 0); // Place the unit to the new tile position
    }

    private void createSelectionRing()
    {
        selectedRing = Instantiate(selectedRing);
        selectedRing.transform.SetParent(gameObject.transform);
        selectedRing.transform.localPosition = new Vector3(0, selectedRing.transform.localPosition.y, 0);
        selectedRing.GetComponent<MeshRenderer>().enabled = false;
    }

    public void IsSelected()
    {
        // Feedback to user that he selected this unit
        selectedRing.GetComponent<MeshRenderer>().enabled = true;
    }

    public void removeSelectionFeedback()
    {
        selectedRing.GetComponent<MeshRenderer>().enabled = false;

        // Changes Tile color
        //currentTile = gameController.GetComponent<BoardController>().getTile(new Vector2(x, y)); // Get the tile using it's current x and y positions
        //currentTile.GetComponent<MeshRenderer>().material = materialNotSelected; // Change the material of the tile
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
