using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains everything related to moving on a global setting:
/// 
/// 1. Getting mouse input
/// 2. Detecting unit selection and deselection
/// 3. Send the move command to the corresponding unit
/// 
/// ----TODO: 
/// 
/// 1. Check if player wants to deseslect first -- DONE
/// </summary>

public class TileDetector : MonoBehaviour
{
    public Material materialSelected;
    public Material materialNotSelected;

    Vector2 mouseOver;
    private bool mouseHasBeenDown;

    // For unit selection
    private bool unitHasBeenSelected;
    private GameObject unitSelected;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Location Script Started");
    }

    // Update is called once per frame
    void Update()
    {
        // Clickdetection
        if (Input.GetMouseButton(0))
        {
            if (mouseHasBeenDown == false)
            {
                UpdateMouseOver();
                mouseHasBeenDown = true;
            }
        }
        else
            mouseHasBeenDown = false;

        // Deselects the unit
        if (Input.GetMouseButton(1) && unitHasBeenSelected == true)
        {
            unitHasBeenSelected = false;
            unitSelected.GetComponent<UnitController>().removeSelectionFeedback();
        }
    }


    private void UpdateMouseOver()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) // Gets the position of the mouse in X and Y coordinates
        {
            mouseOver.x = hit.transform.GetComponent<HexagonScript>().getX();
            mouseOver.y = hit.transform.GetComponent<HexagonScript>().getY();

            //Debug.Log(mouseOver);

            GameObject hoverOverTile = gameObject.GetComponent<BoardController>().getTile(mouseOver); // Gets the tile ID it is hovering over
            GameObject hoverOverUnit = gameObject.GetComponent<BoardController>().getUnit(mouseOver); // Gets the unit that is on the tile (otherwise returns null)

            if (hoverOverTile) // If there is tile
            {
                if (hoverOverUnit && unitHasBeenSelected == false) // When selecting a unit, and makes sure that if a unit has been selected it does not run again
                {
                    unitHasBeenSelected = true;
                    unitSelected = hoverOverUnit;
                    unitSelected.GetComponent<UnitController>().IsSelected();
                }

                else if(unitHasBeenSelected == true) // When moving and deselecting a unit
                {
                    unitHasBeenSelected = false;
                    // ----TODO: Check if player wants to deseslect first

                    // Tell the unit where to go, filter out here already if it is not possible
                    unitSelected.GetComponent<UnitController>().CalculatePath((int)mouseOver.x, (int)mouseOver.y);
                }
            }
        }
    }
}
