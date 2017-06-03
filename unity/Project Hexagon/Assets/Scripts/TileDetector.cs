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
    private int myPlayerID;
    private int myTeamID;

    public Material materialSelected;
    public Material materialNotSelected;

    Vector2 mouseOver;
    private bool mouseHasBeenDown;

    // For unit selection
    private bool unitHasBeenSelected;
    private GameObject unitSelected;
    private GameObject prevUnitSelected;

    // Use this for initialization
    void Start()
    {
        myPlayerID = 0;
        myTeamID = 0;
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
            unitSelected.GetComponent<UnitController>().removeSelectionFeedback();
            unitHasBeenSelected = false;
            unitSelected = new GameObject();
        }

        //select unit shortcut
        if (Input.GetKeyDown("1"))
        {
            int unitsInTeam = 3;
            int teamOffset = myTeamID * unitsInTeam;
            GetComponent<TileDetector>().selectPlayerUnit(GetComponent<BoardController>().getUnitList()[teamOffset]);
        }
        if (Input.GetKeyDown("2"))
        {
            int unitsInTeam = 3;
            int teamOffset = myTeamID * unitsInTeam;
            GetComponent<TileDetector>().selectPlayerUnit(GetComponent<BoardController>().getUnitList()[1+teamOffset]);
        }
        if (Input.GetKeyDown("3"))
        {
            int unitsInTeam = 3;
            int teamOffset = myTeamID * unitsInTeam;
            GetComponent<TileDetector>().selectPlayerUnit(GetComponent<BoardController>().getUnitList()[2+teamOffset]);
        }

        if (Input.GetKeyDown("s"))
        {
            // Switch player ID
            if (myPlayerID == 0)
            {
                myPlayerID = 1;
                myTeamID = 1;
            }
            else
            {
                myPlayerID = 0;
                myTeamID = 0;
            }
            Debug.Log("team = " + myTeamID);
        }
    }


    private void UpdateMouseOver()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) // Gets the position of the mouse in X and Y coordinates
        {
            mouseOver.x = hit.transform.GetComponent<HexagonScript>().getX();
            mouseOver.y = hit.transform.GetComponent<HexagonScript>().getY();


            // guarantee that the previously 
            if (prevUnitSelected && prevUnitSelected != unitSelected)
            {
                prevUnitSelected.GetComponent<UnitController>().hidePathFeedback();
                prevUnitSelected = null;
            }


            //Debug.Log(mouseOver);

            GameObject hoverOverTile = gameObject.GetComponent<BoardController>().getTile(mouseOver); // Gets the tile ID it is hovering over
            GameObject hoverOverUnit = gameObject.GetComponent<BoardController>().getUnit(mouseOver); // Gets the unit that is on the tile (otherwise returns null)

            if (hoverOverTile) // If there is tile
            {
                if (hoverOverUnit && hoverOverUnit != unitSelected) // When selecting a unit, and makes sure that if a unit has been selected
                {
                    int unitPlayerID = hoverOverUnit.GetComponent<UnitController>().getPlayerID();
                    int unitTeamID = hoverOverUnit.GetComponent<UnitController>().getTeamID();

                    // If your unit
                    if (unitPlayerID == myPlayerID)
                    {
                        selectPlayerUnit(hoverOverUnit);
                        return;
                    }

                    // If your teammates unit
                    if (unitPlayerID != myPlayerID && unitTeamID == myTeamID)
                    {

                    }

                    // if enemy's unit
                    if (unitPlayerID != myPlayerID && unitTeamID != myTeamID)
                    {
                        if (unitHasBeenSelected == true)
                        {
                            unitSelected.GetComponent<UnitController>().setUnitGoal(hoverOverUnit, 1); // send the target to attack!
                            unitSelected.GetComponent<UnitController>().showPathFeedback(); // show the created path
                            unitHasBeenSelected = false;
                            prevUnitSelected = unitSelected; //set prev object for deselection
                            unitSelected = null; // Empty the current object
                        }
                        else
                        {

                        }
                    }
                }

                // If empty space is selected
                else if (unitHasBeenSelected == true) // When moving and deselecting a unit
                {
                    // Tell the unit where to go, filter out here already if it is not possible
                    unitSelected.GetComponent<UnitController>().setTileGoal((int)mouseOver.x, (int)mouseOver.y, 0);
                    unitSelected.GetComponent<UnitController>().showPathFeedback(); // show the created path
                    unitHasBeenSelected = false;
                    prevUnitSelected = unitSelected; //set prev object for deselection
                    unitSelected = null; // Empty the current object
                }
            }
        }   
        // If no tile, deselect all units and remove their feedback
        else
        {
            unitHasBeenSelected = false;
            if (unitSelected) {
                unitSelected.GetComponent<UnitController>().hidePathFeedback();
                unitSelected.GetComponent<UnitController>().removeSelectionFeedback();
                unitSelected = null;
            }if (prevUnitSelected) {
                prevUnitSelected.GetComponent<UnitController>().hidePathFeedback();
            prevUnitSelected.GetComponent<UnitController>().removeSelectionFeedback();
                prevUnitSelected = null;
            }
        }
    }

    public void selectPlayerUnit(GameObject unitToSelect)
    {
        if (unitHasBeenSelected == true)
        {
            unitSelected.GetComponent<UnitController>().removeSelectionFeedback();
            unitSelected.GetComponent<UnitController>().hidePathFeedback();
        }
        unitHasBeenSelected = true;
        unitSelected = unitToSelect; // change the unit selected
        unitSelected.GetComponent<UnitController>().IsSelected();
        unitSelected.GetComponent<UnitController>().showPathFeedback();
    }

    public int getTeamID()
    {
        return myTeamID;
    }
}
