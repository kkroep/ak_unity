using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }


    private void UpdateMouseOver()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            mouseOver.x = hit.transform.GetComponent<HexagonScript>().getX();
            mouseOver.y = hit.transform.GetComponent<HexagonScript>().getY();

            //Debug.Log(mouseOver);

            GameObject hoverOverTile = gameObject.GetComponent<GenerateLevel>().getTile(mouseOver);
            GameObject hoverOverUnit = gameObject.GetComponent<GenerateLevel>().getUnit(mouseOver);

            if (hoverOverTile)
            {
                if (hoverOverUnit)
                {
                    unitHasBeenSelected = true;
                    unitSelected = hoverOverUnit;
                    hoverOverTile.GetComponent<MeshRenderer>().material = materialSelected;
                }
                else if(unitHasBeenSelected == true)
                {
                    unitHasBeenSelected = false;
                    int old_x = unitSelected.GetComponent<UnitController>().getX();
                    int old_y = unitSelected.GetComponent<UnitController>().getY();
                    
                    GameObject old_tile = gameObject.GetComponent<GenerateLevel>().getTile(new Vector2(old_x, old_y));
                    old_tile.GetComponent<MeshRenderer>().material = materialNotSelected;
                    gameObject.GetComponent<GenerateLevel>().deleteUnit(old_x,old_y);
                    gameObject.GetComponent<GenerateLevel>().setUnit((int)mouseOver.x, (int)mouseOver.y, unitSelected);


                    unitSelected.transform.position = hit.transform.position + new Vector3(0, unitSelected.transform.position.y, 0);
                }
            }
        }
    }
}
