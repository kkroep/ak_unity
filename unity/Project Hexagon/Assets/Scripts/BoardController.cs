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
    public GameObject mountainPrefab;
    public GameObject forrestPrefab;
    public GameObject AIPrefab;
    public GameObject pathingRing;
    private HexMath hexMath;
    private GameObject[,] tileMatrix; //Contains the tiles and their locations

    public int[] boardsize = new int[2];

    // Unit Matrix
    private List<GameObject> unitList = new List<GameObject>();
    private List<GameObject> unitList_T1 = new List<GameObject>();
    private List<GameObject> unitList_T2 = new List<GameObject>();
    private List<GameObject> nextUnitList = new List<GameObject>();
    private GameObject[,] unitMatrix; //Contains the units
    private GameObject[,] nextStepMatrix; // Contains the position of the units in the next step
    public GameObject Hoplite;
    public GameObject Archer;

    //reading map
    public TextAsset mapTextFile;
    public int[,] tileProperties;

    // Artificial Intelligence
    private GameObject AI;

    void Start ()
    {
        // sequence to parse textfile
        string txtFileAsOneLine = mapTextFile.text;
        string[] oneLine;
        List<string> txtFileAsLines = new List<string>();
        txtFileAsLines.AddRange(txtFileAsOneLine.Split("\n"[0]));
        oneLine = txtFileAsLines[0].Split(" "[0]);
        Debug.Log(oneLine[0]);

        boardsize[0] = int.Parse(oneLine[0]);
        boardsize[1] = int.Parse(oneLine[1]);
        Debug.Log(boardsize[0]+","+boardsize[1]);

        tileProperties = new int[boardsize[0], boardsize[1]];
        for (int j = 0; j < boardsize[1]; j++)
        {
            oneLine = txtFileAsLines[1 + j].Split(" "[0]);
            for (int i = 0; i < boardsize[0]; i++)
            {
                tileProperties[i, j] = int.Parse(oneLine[i]);
            }
        }


        tileMatrix = new GameObject[boardsize[0], boardsize[1]];
        unitMatrix = new GameObject[boardsize[0], boardsize[1]];
        nextStepMatrix = new GameObject[boardsize[0], boardsize[1]];
        hexMath = gameObject.GetComponent<HexMath>(); // Takes the HexMath script from the Game Control
    


        for (int i = 0; i < boardsize[0]; i++) // Generate the X hexagons
        {
            float x = hexMath.matrix2HexX(i);

            for (int j = 0; j < boardsize[1]; j++) // Generate the Y hexagons
            {
                float y = hexMath.matrix2HexY(i, j);
                
                GameObject hexagon = Instantiate(HexagonTile);
                hexagon.transform.SetParent(transform); // Puts the tile under TileManager and gives the same transform
                hexagon.transform.localPosition = new Vector3(x,0,y);
                hexagon.GetComponent<HexagonScript>().set(i, j);

                if (tileProperties[i,j]==0)
                {
                    //empty
            
                    hexagon.GetComponent<MeshRenderer>().enabled = false;
                    hexagon.GetComponent<MeshCollider>().enabled = false;
                    hexagon.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                }
                if (tileProperties[i, j] == 2) {
                    //mountain
                    GameObject mountain = Instantiate(mountainPrefab);
                    mountain.transform.SetParent(transform); // Puts the tile under TileManager and gives the same transform
                    mountain.transform.localPosition = new Vector3(x,0,y);
                    hexagon.GetComponent<MeshCollider>().enabled = false;
                    hexagon.GetComponent<MeshRenderer>().enabled = false;
                }

                if (tileProperties[i, j] == 3) {
                    //forrest
                    GameObject forrest = Instantiate(forrestPrefab);
                    forrest.transform.SetParent(transform); // Puts the tile under TileManager and gives the same transform
                    forrest.transform.localPosition = new Vector3(x,0,y);
                }

                // Create pathing ring on it instantly
                GameObject pathing = Instantiate(pathingRing);
                pathing.transform.SetParent(hexagon.transform);
                pathing.transform.localPosition = new Vector3(0, 0, 0.05f);
                pathing.GetComponent<MeshRenderer>().enabled = false;

                tileMatrix[i, j] = hexagon;
            }
        }

        int[] unitLoc = new int[2] {0,0};
        // Spawn the first units


        unitLoc = new int[2] { 3, 8 };
        GameObject Unit = Instantiate(Hoplite);
        Unit.GetComponent<UnitController>().set(unitLoc[0],unitLoc[1]);
        unitMatrix[unitLoc[0], unitLoc[1]] = Unit;
        Unit.transform.position = tileMatrix[unitLoc[0],unitLoc[1]].transform.position;
        unitList_T1.Add(Unit);
        unitList.Add(Unit);
        Unit.GetComponent<UnitController>().setPlayerID(0);
        Unit.GetComponent<UnitController>().setTeamID(0);

        unitLoc = new int[2] { 4, 4 };
        Unit = Instantiate(Archer);
        Unit.GetComponent<UnitController>().set(unitLoc[0],unitLoc[1]);
        unitMatrix[unitLoc[0], unitLoc[1]] = Unit;
        Unit.transform.position = tileMatrix[unitLoc[0],unitLoc[1]].transform.position;
        unitList_T1.Add(Unit);
        unitList.Add(Unit);
        Unit.GetComponent<UnitController>().setPlayerID(0);
        Unit.GetComponent<UnitController>().setTeamID(0);

        unitLoc = new int[2] { 12, 12 };
        Unit = Instantiate(Archer);
        Unit.GetComponent<UnitController>().set(unitLoc[0],unitLoc[1]);
        unitMatrix[unitLoc[0], unitLoc[1]] = Unit;
        Unit.transform.position = tileMatrix[unitLoc[0],unitLoc[1]].transform.position;
        unitList_T2.Add(Unit);
        unitList.Add(Unit);
        Unit.GetComponent<UnitController>().setPlayerID(1);
        Unit.GetComponent<UnitController>().setTeamID(1);

        unitLoc = new int[2] { 13, 4 };
        Unit = Instantiate(Archer);
        Unit.GetComponent<UnitController>().set(unitLoc[0],unitLoc[1]);
        unitMatrix[unitLoc[0], unitLoc[1]] = Unit;
        Unit.transform.position = tileMatrix[unitLoc[0],unitLoc[1]].transform.position;
        unitList_T2.Add(Unit);
        unitList.Add(Unit);
        Unit.GetComponent<UnitController>().setPlayerID(1);
        Unit.GetComponent<UnitController>().setTeamID(1);

        //GameObject Hoplite2 = Instantiate(Unit);
        //Hoplite2.GetComponent<UnitController>().set(4, 4);
        //Hoplite2.transform.position = new Vector3( GetComponent<HexMath>().matrix2HexX(4), Hoplite2.transform.position.y, GetComponent<HexMath>().matrix2HexY(4,4));
        //unitMatrix[4, 4] = Hoplite2;
        //unitList.Add(Hoplite2);

        // Initialize Artidicial Intelligence to be player 2
        AI = Instantiate(AIPrefab);
        AI.GetComponent<AI>().initPlayer(1, unitList_T2, unitList_T1);


        // Start the timer
        StartCoroutine(TurnTimer()); // Start the turntimer!!!
    }
	

	// Update is called once per frame
	void Update ()
    {
        // each time spacebar is pressed, make units move
        if (Input.GetKeyDown("space"))
        {
            nextUnitList = unitList;

            if (unitList.Count == 0 || unitList_T2.Count == 0)
            {
                Debug.Log("Game Over!");
                return;
            }
            

            foreach (GameObject selectedUnit in unitList.ToArray())
                {
                    selectedUnit.GetComponent<UnitController>().resetAP();
                }
            foreach (GameObject selectedUnit in unitList_T2.ToArray())
                {
                    selectedUnit.GetComponent<UnitController>().resetAP();
                }

            // Call the attack order of each object in the list
            for (int AP = 5; AP > 0; AP--)
            {
                // Update the new unit positions if needed
                foreach (GameObject selectedUnit in unitList.ToArray())
                {
                    selectedUnit.GetComponent<UnitController>().nextAttack(AP);
                }


                foreach (GameObject selectedUnit in unitList.ToArray())
                {
                    selectedUnit.GetComponent<UnitController>().executeNextAttack(AP);
                }


                // Check who dieded
                foreach (GameObject selectedUnit in unitList.ToArray())
                {
                    selectedUnit.GetComponent<UnitController>().nextDieded();
                }


                if (unitList_T1.Count == 0 || unitList_T2.Count == 0)
                {
                    if (unitList_T1.Count == 0)
                        Debug.Log("player 2 won");
                    else
                        Debug.Log("player 1 won");
                    return;
                }

                unitList = nextUnitList;

                // Calculate the next step the unit is going to make, and try to claim a position in the new matrix. Conflicts are all resolved here!
                foreach (GameObject selectedUnit in unitList.ToArray())
                {
                    selectedUnit.GetComponent<UnitController>().nextStep(AP);
                }

                unitMatrix = nextStepMatrix;
                nextStepMatrix = new GameObject[boardsize[0], boardsize[1]]; // clear the nextStepMatrix

                // Call the movement of each object in the list
                foreach (GameObject selectedUnit in unitList.ToArray())
                {
                    selectedUnit.GetComponent<UnitController>().executeNextStep();
                }
            }
            AI.GetComponent<AI>().nextTurn();
        }
    }

    IEnumerator TurnTimer()
    {
        while(true)
        {
            //Debug.Log("Before Waiting 10 seconds");
            yield return new WaitForSeconds(10); // Calls for the function WaitForSeconds. Yeild break breaks this.
            //Debug.Log("After Waiting 10 Seconds");
            
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

    public GameObject getNextStepLocation(int x, int y)
    {
        return nextStepMatrix[x, y];
    }

    public void setNextStepLocation(GameObject claimingUnit, int x, int y)
    {
        nextStepMatrix[x, y] = claimingUnit;
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

    public void removeFromUnitList(GameObject deadUnit)
    {
        nextUnitList.Remove(deadUnit);
    }
}
