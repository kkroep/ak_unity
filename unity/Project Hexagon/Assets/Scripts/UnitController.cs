﻿using System.Collections;
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
    protected int playerID;
    protected int teamID;

    protected int x, y; // Current coordinates of a certain unit
    protected int next_x, next_y;
    protected int priority;
    protected float health;
    protected int turn;
    protected int attack;
    List<int[]> moveQueue = new List<int[]>(); // The list of places if it needs to move, else it's empty

    // Flags
    protected int turnsDoneNothing;
    protected bool defenseModeEnabled;

    // Goals
    protected GameObject attackTarget;
    protected int[] goalCoordinates;
    protected int goalType;

    // External set information
    public Material materialSelected;
    public Material materialNotSelected;
    public GameObject selectedRing;

    // Global variables
    protected GameObject gameController;
    protected GameObject currentTile;
    protected int[,] boardSize;
    protected int[,] neighbors;
    protected HexMath hexMath;

    // flags for end turn mechanics
    bool attackOnGround;
    bool hasMoved;
    bool scheduledSkillMovement;
    bool scheduledAttack;
    bool scheduledHit;
    bool scheduledNormalMovement;

    void Start()
    {
        neighbors = new int[6, 2] {                 { 0, 1 },
                                                    { 1, 0 },
                                                    { 1, -1 },
                                                    { 0, -1 },
                                                    { -1, 0 },
                                                    { -1, 1 }};


        gameController = GameObject.FindGameObjectWithTag("GameController");
        hexMath = gameController.GetComponent<HexMath>(); // Takes the HexMath script from the Game Control
        boardSize = new int[gameController.GetComponent<BoardController>().boardsize[0], gameController.GetComponent<BoardController>().boardsize[1]];

        //Create the ring that determines the feedback, hide it when not selected
        createSelectionRing();

        turn = 0;
        setUnitParameters();
    }

    protected virtual void setUnitParameters() {
        health = 100;
        attack = 25;
        return;
    }

    // Calculates the path using the Dijkstra algorithm
    public void CalculatePath()
    {
        int[] oldCoordinates = new int[2] { x, y };
        int[] newCoordinates = new int[2] { goalCoordinates[0], goalCoordinates[1] };

        // Removing the selection visual feedback
        removeSelectionFeedback();

        // Check if there has been a previous movement path, if yes, clear the feedback of it
        hidePathFeedback();

        // Check if player wants to cancel the move order by pressing the unit again
        if (goalCoordinates[0] == x && goalCoordinates[1] == y)
        {
            moveQueue.Clear(); // Clear the queue list
            return;
        }

        // Calculating the new movement path
        moveQueue = gameController.GetComponent<Dijkstra>().route(boardSize, oldCoordinates, newCoordinates);
    }

    // Type indicates what type of skill is used
    public void setTileGoal(int x_new, int y_new, int newGoalType)
    {
        goalType = newGoalType;
        goalCoordinates = new int[] { x_new, y_new };
        attackTarget = null;
        CalculatePath();
    }

    public void setUnitGoal(GameObject new_Target, int newGoalType)
    {
        goalType = newGoalType;
        attackTarget = new_Target;
        goalCoordinates = attackTarget.GetComponent<UnitController>().getCurrentPosition(); // set new goal
        CalculatePath(); // calculate path towards attack goal
    }

    /* handle everything that should happen before the beginning of the next attack sequence
     */
    public void beginTurn()
    {
        // reset logic scheduling flags
        hasMoved = false;
        attackOnGround = false;

        // reset animation scheduling flags
        scheduledSkillMovement = false;
        scheduledAttack = false;
        scheduledHit = false;
        scheduledNormalMovement = false;
    }
    
    /* Check whether the unit wanhts to perform a missile attack and if the target is in range
     * If the target is in range, schedule an attack on ground
     * Finally set the goal to staying in the place it currently is in
     */
    public void scheduleMissileAttack()
    {
        //if it already did something this turn
        if (hasMoved)
            return;
    }

    /* If a skill involving movement is executed, 
     * move the character logically, 
     * and schedule an execute skill movement with a type
            return;
     * The character model doesn't move in this function
     */
    public void skillMovement()
    {
        if (hasMoved)
            return;
    }

    /* If an attack on ground is scheduled, 
     * perform the attack, and calculate what units are hit with what damage
     * then schedule an animation for the attack ground
     */
    public void attackGround()
    {
        if (hasMoved)
            return;
    }

    /* Figure out whether the target is in range 
     * and if so immediately attack the target
     */
    public void attackMelee()
    {
        if (hasMoved)
            return;
    }

    /* calculate all the effects of being hit and schedule the appropriate animations for it
     * also check if the unit died this turn, together with scheduling the death animation
     */
    public void calculateHit()
    {
        if (hasMoved)
            return;
    }

    /* If the unit has done nothing at this point and still wants to move normally it can do so here
     * The unit is logically moved, but the animation is scheduled
     */
    public void normalMovement()
    {
        if (hasMoved)
            return;
    }

    /* execute the animation for skill movement if scheduled
     */
    public void animateSkillMovement()
    {
        if (scheduledSkillMovement)
            return;
    }

    /* execute the animation for attack if scheduled
     */
    public void animateAttack()
    {
        if (scheduledAttack)
            return;
    }

    /* execute the animation for being hit (and maybe died) if scheduled
     */
    public void animateHit()
    {
        if (scheduledHit)
            return;
    }

    /* execute the animation for normal movement if scheduled
     */
    public void animateNormalMovement()
    {
        if (scheduledNormalMovement)
            return;
    }






    public void nextAttack()
    {
        if (goalType == 0)
            return;
        // normal attack is with target and and goalType 1. If any attack has no target. remove the target.
        if (attackTarget == null)
        {
            setTileGoal(x, y, 0); // if auto attacking, but has no target proceed to stop moving entirely
            return;
        }
        else
        {
            // If target is dead -> cancel current moveorder, and clear the target and return
            if (attackTarget.GetComponent<UnitController>().getHealth() <= 0)
            {
                moveQueue.Clear();
                goalCoordinates = new int[] { x, y };
                attackTarget = null;
                return;
            }
            // update goal
            goalCoordinates = attackTarget.GetComponent<UnitController>().getCurrentPosition();

            // if path endpoint != goal, recalculate path to the enemy target
            if (moveQueue.Count != 0)
            {
                // reason for the else is that if movequeue is empty, it will give an exception error in the statement under
                if (moveQueue[moveQueue.Count - 1] != goalCoordinates)
            {
                    CalculatePath();
                }
            }
            else
            {
                CalculatePath();
            }
        }
}

    public virtual void executeNextAttack()
    {
        // if has target
        if (attackTarget == null)
            return;
        else
        {
            // Check if can het target, if yes, attack

            int diffx = goalCoordinates[0] - x;
            int diffy = goalCoordinates[1] - y;

            for (int i = 0; i < 6; i++)
            {
                if (neighbors[i, 0] == diffx && neighbors[i, 1] == diffy)
                {
                    // ATTACK
                    attackTarget.GetComponent<UnitController>().reduceHealth((float)attack);
                }
            }
        }
    }


    public void nextDieded()
    {
        // First check if unit DIEDED
        if (health == 0)
        {
            killYourself(); // it dieded
        }
    }

    public void nextStep()
    {
        // If unit does not want to move, execute this shit
        if (moveQueue.Count == 0)
        {
            priority = 100;
            next_x = x;
            next_y = y;
        }
        else
        {
            priority = 0;
            next_x = moveQueue[0][0]; // Try to claim this position
            next_y = moveQueue[0][1];
        }

        // Get the potential unit from the unitMatrix
        GameObject potentialUnit = gameController.GetComponent<BoardController>().getNextStepLocation(next_x, next_y);

        // If spot is empty, claim it, and write it to the nextStepMatrix! Then write in it's memory it's potential next step
        if (potentialUnit == null)
        {
            gameController.GetComponent<BoardController>().setNextStepLocation(gameObject, next_x, next_y);
            GetComponent<AreaModule>().Update_FoV(next_x,next_y);
            if (moveQueue.Count != 0)
            {
                moveQueue.RemoveAt(0); // Remove executed entry from the queue
            }
        }

        // Resolve conflict
        else
        {
            if (potentialUnit.GetComponent<UnitController>().getPriority() >= priority)
            {
                // If other unit it's priority is higher or was earlier than you, wait!
                gameController.GetComponent<BoardController>().setNextStepLocation(gameObject, x, y);
                next_x = x;
                next_y = y;
            }

            // If other unit's priority is lower, tell him to step back and claim the position
            else
            {
                gameController.GetComponent<BoardController>().setNextStepLocation(gameObject, next_x, next_y);
                potentialUnit.GetComponent<UnitController>().stepBack();
            }
        }
    }

    public void executeNextStep()
    {
        // If unit does not want to move, execute this shit
        if (next_x == x && next_y == y)
        {
            return;
            turnsDoneNothing++;
            // Defense mode here
            enableDefenseMode();
        }
        else
        {

            // Reset turnsDoneNothing, and disable defense mode. We keep defense mode disabled, also if the unit has to wait one turn to step!
            turnsDoneNothing = 0;
            disableDefenseMode();

            // Get the new tile remove feedback (thus position)
            GameObject newTile = gameController.GetComponent<BoardController>().getTile(next_x, next_y); // Get the new tile
            newTile.gameObject.transform.Find("PathingRing(Clone)").gameObject.GetComponent<MeshRenderer>().enabled = false;

            // Move the unit to the new tile
            gameController.GetComponent<BoardController>().setUnit(next_x, next_y, gameObject); // Sets THIS unit to new position in the matrix
            x = next_x; // Set new unit coordinates
            y = next_y;
            transform.position = newTile.transform.position + new Vector3(0, transform.position.y, 0); // Place the unit to the new tile position
        }
    }

    private void enableDefenseMode()
    {
        if (turnsDoneNothing == 2)
        {

        }
    }


    private void disableDefenseMode()
    {

    }

    public void stepBack()
    {
        moveQueue.Insert(0, new int[] { next_x, next_y });
        nextStep();
    }


    public void showPathFeedback()
    {
        foreach (int[] location in moveQueue)
        {
            currentTile = gameController.GetComponent<BoardController>().getTile(new Vector2(location[0], location[1])); // Get the tile
            currentTile.gameObject.transform.Find("PathingRing(Clone)").gameObject.GetComponent<MeshRenderer>().enabled = true; // Enable the ring
        }
    }

    public void hidePathFeedback()
    {
        foreach (int[] location in moveQueue)
        {
            currentTile = gameController.GetComponent<BoardController>().getTile(new Vector2(location[0], location[1])); // Get the tile
            currentTile.gameObject.transform.Find("PathingRing(Clone)").gameObject.GetComponent<MeshRenderer>().enabled = false; // Disable the ring
        }
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

    public int getPlayerID()
    {
        return playerID;
    }

    public void setPlayerID(int newID)
    {
        playerID = newID;
    }

    public int getTeamID()
    {
        return teamID;
    }

    public void setTeamID(int newID)
    {
        teamID = newID;
    }

    public void set(int new_x, int new_y)
    {
        x = new_x;
        y = new_y;
        next_x = x;
        next_y = y;
    }

    public int getPriority()
    {
        return priority;
    }

    public int[] getCurrentPosition()
    {
        int[] ans = { x, y };
        return ans;
    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public float getHealth()
    {
        return health;
    }

    public void reduceHealth(float damageTaken)
    {
        health = health - damageTaken;
        Debug.Log(health);
        if (health <= 0)
        {
            health = 0;
        }
    }

    public void killYourself()
    {
        gameController.GetComponent<BoardController>().removeFromUnitList(gameObject);
        gameController.GetComponent<BoardController>().setUnit(x, y, null);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Destroy(transform.GetChild(0).gameObject);

        hidePathFeedback();
        moveQueue = null;

        goalCoordinates = null;
        attackTarget = null;

    }
}
