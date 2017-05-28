using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
    private int playerID;
    private List<GameObject> unitList = new List<GameObject>();
    private List<GameObject> enemyUnitList = new List<GameObject>();

    protected GameObject gameController;
    // Use this for initialization
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initPlayer(int new_playerID, List<GameObject> new_unitList, List<GameObject> new_enemyUnitList)
    {
        playerID = new_playerID;
        unitList = new_unitList;
        enemyUnitList = new_enemyUnitList;
        return;
    }

    public void nextTurn()
    {
        if (unitList.Count == 0)
        {
            Debug.Log("I beat you blue!");
            return;
        }
        foreach (var item in unitList)
        {
            item.GetComponent<UnitController>().setUnitGoal(enemyUnitList[0]);
        }
        return;
    }
}
