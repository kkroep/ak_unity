using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    // Board, Hexagons
    public GameObject HexagonTile;
    HexMath hexMath;
    GameObject[,] tileMatrix;

    public int[] boardsize = new int[2];

    // Unit Matrix
    GameObject[,] unitMatrix;
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

                tileMatrix[i, j] = hexagon;
            }
        }

        // Spawn the first units
        GameObject Hoplite = Instantiate(Unit);
        Hoplite.GetComponent<UnitController>().set(0,0);
        unitMatrix[0, 0] = Hoplite;


    }
	

	// Update is called once per frame
	void Update ()
    {
		
	}

    public GameObject getTile(Vector2 mouseOver)
    {
        return tileMatrix[(int)mouseOver.x, (int)mouseOver.y];
    }

    public void setUnit(int x, int y, GameObject new_Unit)
    {
        unitMatrix[x, y] = new_Unit;
    }

    public GameObject getUnit(Vector2 mouseOver)
    {
        return unitMatrix[(int)mouseOver.x, (int)mouseOver.y];
    }
    public void deleteUnit(int x, int y)
    {
        Debug.Log(unitMatrix[x, y]);
        unitMatrix[x, y] = null;
        Debug.Log(unitMatrix[x, y]);
    }
}
