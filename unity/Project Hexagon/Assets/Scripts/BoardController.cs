using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains the global board controller:
/// 
/// 1. Generating the board at Start()
/// 2. The unit and board Matrixes, containing the current turns positions of all the elements
/// 3. Contain the clock
/// 
/// ----TODO:
/// 
/// 1. 
/// 
/// ----Milestone: Multiple units can move around without stepping on the same hexagon
/// </summary>

public class BoardController : MonoBehaviour
{
    // Board, Hexagons
    public GameObject HexagonTile;
    public GameObject pathingRing;
    HexMath hexMath;
    GameObject[,] tileMatrix; //Contains the tiles and their locations

    public int[] boardsize = new int[2];

    // Unit Matrix
    List<GameObject> unitList = new List<GameObject>();
    GameObject[,] unitMatrix; //Contains the units
    public GameObject Unit;



    void Start ()
    {
        tileMatrix = new GameObject[boardsize[0], boardsize[1]];
        unitMatrix = new GameObject[boardsize[0], boardsize[1]];
        hexMath = gameObject.GetComponent<HexMath>(); // Takes the HexMath script from the Game Controll

        for (int i = 0; i < boardsize[0]; i++) // Generate the X hegagons
        {
            float x = hexMath.matrix2HexX(i);

            for (int j = 0; j < boardsize[1]; j++) // Generate the Y hexagons
            {
                float y = hexMath.matrix2HexY(i, j);
                
                GameObject hexagon = Instantiate(HexagonTile);
                hexagon.transform.SetParent(transform); // Puts the tile under TileManager and gives the same transform
                hexagon.transform.localPosition = new Vector3(x,0,y);
                hexagon.GetComponent<HexagonScript>().set(i, j);

                // Create pathing ring on it instantly
                GameObject pathing = Instantiate(pathingRing);
                pathing.transform.SetParent(hexagon.transform);
                pathing.transform.localPosition = new Vector3(0, 0, 0.05f);
                pathing.GetComponent<MeshRenderer>().enabled = false;

                tileMatrix[i, j] = hexagon;
            }
        }

        // Spawn the first units
        GameObject Hoplite = Instantiate(Unit);
        Hoplite.GetComponent<UnitController>().set(0,0);
        unitMatrix[0, 0] = Hoplite;
        unitList.Add(Hoplite);

        GameObject Hoplite2 = Instantiate(Unit);
        Hoplite2.GetComponent<UnitController>().set(4, 4);
        Hoplite2.transform.position = new Vector3( GetComponent<HexMath>().matrix2HexX(4), Hoplite2.transform.position.y, GetComponent<HexMath>().matrix2HexY(4,4));
        unitMatrix[4, 4] = Hoplite2;
        unitList.Add(Hoplite2);

        // Start the timer

        StartCoroutine(TurnTimer()); // Start the turntimer!!!
    }
	

	// Update is called once per frame
	void Update ()
    {
        // each time spacebar is pressed, make units move
        if (Input.GetKeyDown("space"))
        {
            // Call the movement of each object in the list
            foreach (GameObject iUnit in unitList)
            {
                Debug.Log("hoi");
                iUnit.GetComponent<UnitController>().MoveUnit();
            }
        }
    }

    IEnumerator TurnTimer()
    {
        while(true)
        {
            Debug.Log("Before Waiting 10 seconds");
            yield return new WaitForSeconds(10); // Calls for the function WaitForSeconds. Yeild break breaks this.
            Debug.Log("After Waiting 10 Seconds");
            
            // ----TODO: We need to get an list which orders all the movements of the units, and write an algorithm which sorts out all the rules, sets attack declarations etc.

        }
    }

    private void createPathingRing()
    {
        pathingRing = Instantiate(pathingRing);
        pathingRing.transform.SetParent(gameObject.transform);
        pathingRing.transform.localPosition = new Vector3(0, pathingRing.transform.localPosition.y, 0);
        pathingRing.GetComponent<MeshRenderer>().enabled = false;
    }

    public GameObject getTile(Vector2 mouseOver)
    {
        return tileMatrix[(int)mouseOver.x, (int)mouseOver.y];
    }

    public GameObject getTile(int x, int y)
    {
        return tileMatrix[x, y];
    }

    public void setUnit(int x, int y, GameObject new_Unit)
    {
        unitMatrix[x, y] = new_Unit;
    }

    public GameObject getUnit(Vector2 mouseOver)
    {
        return unitMatrix[(int)mouseOver.x, (int)mouseOver.y];
    }

    public GameObject getUnit(int x, int y)
    {
        return unitMatrix[x, y];
    }

    public void deleteUnit(int x, int y)
    {
        Debug.Log(unitMatrix[x, y]);
        unitMatrix[x, y] = null;
        Debug.Log(unitMatrix[x, y]);
    }
}
