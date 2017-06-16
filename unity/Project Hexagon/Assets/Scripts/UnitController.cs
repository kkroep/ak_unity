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
    protected int playerID;
    protected int teamID;

    protected int x, y; // Current coordinates of a certain unit
    protected Vector3 world_position; // Current coordinates of a certain unit for animations
    protected int next_x, next_y;
    protected int priority;
    protected float health;
    protected float speed;
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
    protected bool attackOnGround;
    protected bool hasMoved;
    protected bool hasDied;
    protected bool scheduledSkillMovement;
    protected bool scheduledAttack;
    protected bool scheduledHit;
    protected bool scheduledNormalMovement;

    // aniamtion and skill stuff
    protected ActionController actionController;

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

        turn = 0;
        setUnitParameters();
        hasDied = false;
        speed = 0.12f;
        world_position = transform.position;
        actionController = GetComponent<ActionController>();
    }

    private void Update()

    {
        transform.position = Vector3.MoveTowards(transform.position, world_position, speed);
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
    public void setTileGoal(int x_new, int y_new, int newSkillType)
    {
        //actionController.setSkillType(newSkillType);
        goalCoordinates = new int[] { x_new, y_new };
        attackTarget = null;
        CalculatePath();
    }

    public void setUnitGoal(GameObject new_Target, int newSkillType)
    {
        actionController.setSkillType(newSkillType);
        attackTarget = new_Target;
        goalCoordinates = attackTarget.GetComponent<UnitController>().getCurrentPosition(); // set new goal
        CalculatePath(); // calculate path towards attack goal
    }

    // handle everything that should happen before the beginning of the next attack sequence
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

    // Check whether the unit wanhts to perform a missile attack and if the target is in range
    // If the target is in range, schedule an attack on ground
    // Finally set the goal to staying in the place it currently is in
    public void scheduleMissileAttack()
    {
        //if it already did something this turn
        if (hasMoved || hasDied)
            return;
        if (actionController.checkIfMissileAttack())
        {
            Debug.Log("going for missile");
            goalCoordinates = attackTarget.GetComponent<UnitController>().getCurrentPosition();
            if (hexMath.hexDistance(x - goalCoordinates[0], y - goalCoordinates[1]) < actionController.getAttackRange(1)) {
                attackOnGround = true;
                Debug.Log("kill him!");
            }
        }
    }

    // If a skill involving movement is executed, 
    // move the character logically, 
    // and schedule an execute skill movement with a type
    // The character model doesn't move in this function
    // One more important thing: even if no skill is executed, units standing still will still have to claim positions
    public void skillMovement()
    {
        if (hasMoved || hasDied)
            return;
    }

    // If an attack on ground is scheduled, 
    // perform the attack, and calculate what units are hit with what damage
    // then schedule an animation for the attack ground
    public void attackGround()
    {
        if (!attackOnGround || hasMoved || hasDied)
            return;

        scheduledAttack = true;
        hasMoved = true;
    }


    // Figure out whether the target is in range 
    // and if so immediately attack the target
    public void attackMelee()
    {
        if (hasMoved || hasDied)
            return;

        // TODO: check if attack type is a melee attack. If not , stop the function immediately
        if (goalType != 1)
            return;

        UnitController target = attackTarget.GetComponent<UnitController>();
        if (target.getHealth() <= 0)
        {
            moveQueue.Clear();
            // If the target is already dead, proceed to standing there and idle around
            setTileGoal(x, y, 0);

            return;
        }

        // update goal
        goalCoordinates = target.getCurrentPosition();

        // If in range of target, proceed to melee attack, and scheduling animation
        //Debug.Log(hexMath.hexDistance(x - goalCoordinates[0], y - goalCoordinates[1]));
        if (hexMath.hexDistance(x - goalCoordinates[0], y - goalCoordinates[1]) <= 2) {
            target.reduceHealth((float)attack);
            //target.get
            scheduledAttack = true; // schedule animation
            hasMoved = true;
        }
    }

    // If the unit has done nothing at this point and still wants to move normally it can do so here
    // The unit is logically moved, but the animation is scheduled
    public void normalMovement()
    {
        if (hasDied)
            return;
        // if the unit has already done something, or no desire to change positions, quit out after setting a firm priority
        if (x == goalCoordinates[0] && y == goalCoordinates[1] || hasMoved)
        {
            priority = 100;
            next_x = x;
            next_y = y;
        } else if (moveQueue.Count == 0 || moveQueue[moveQueue.Count - 1] != goalCoordinates)
        {
            // if the path does not need to the desired destination, recalculate the path
            CalculatePath();
            priority = 0;
            next_x = moveQueue[0][0]; // Try to claim this position
            next_y = moveQueue[0][1];
        } else
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
            //GetComponent<AreaModule>().Update_FoV(next_x,next_y);
            scheduledNormalMovement = true; // schedule the animation for a normal movement
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
                priority = 100;
            }

            // If other unit's priority is lower, tell him to step back and claim the position
            else
            {
                gameController.GetComponent<BoardController>().setNextStepLocation(gameObject, next_x, next_y);
                potentialUnit.GetComponent<UnitController>().stepBack();
                scheduledNormalMovement = true;
            }
        }

        // if the unit ended up not mioving, make sure there is no animation
        if (next_x == x && next_y == y)
            scheduledNormalMovement = false;
    }

    // execute the animation for skill movement if scheduled
    public void animateSkillMovement()
    {
        if (scheduledSkillMovement == false)
            return;
    }

    // execute the animation for attack if scheduled
    public void animateAttack()
    {
        if (scheduledAttack == false)
            return;

        actionController.animateAttack(attackTarget.GetComponent<UnitController>().getX(), attackTarget.GetComponent<UnitController>().getY());
    }

    // execute the animation for being hit (and maybe died) if scheduled
    public void animateHit()
    {
        if (scheduledHit == false)
            return;

        // First check if unit DIEDED
        if (health == 0)
        {
            killYourself(); // it dieded
        }
        else {
            actionController.bleed();
        }
    }

    // execute the animation for normal movement if scheduled
    public void animateNormalMovement()
    {
        if (scheduledNormalMovement == false)
            return;

        //Debug.Log("mopving from (" + x + "," + y + ") to (" + next_x + "," + next_y + ")");

        // Get the new tile remove feedback (thus position)
        GameObject newTile = gameController.GetComponent<BoardController>().getTile(next_x, next_y); // Get the new tile
        newTile.gameObject.transform.Find("PathingRing(Clone)").gameObject.GetComponent<MeshRenderer>().enabled = false;

        // Move the unit to the new tile
        gameController.GetComponent<BoardController>().setUnit(next_x, next_y, gameObject); // Sets THIS unit to new position in the matrix


        // get the correct angle
        rotate2Neighbor(next_x - x, next_y - y);

        x = next_x; // Set new unit coordinates
        y = next_y;

        actionController.walk();
        world_position = newTile.transform.position + new Vector3(0, 0, 0); // Place the unit to the new tile position            
        GetComponent<AreaModule>().Update_FoV(next_x, next_y); // Update Field of View
    }

    public void rotate2Neighbor(int x, int y)
    {
        int neighbor_helper = x + y * 10; // to ensure a simple statement can be used for all angles
        switch (neighbor_helper)
        {
            case 0:
                break;
            case -10:
                transform.eulerAngles = new Vector3 { x = 0, y = 0, z = 0 };
                break;
            case -1:
                transform.eulerAngles = new Vector3 { x = 0, y = 60, z = 0 };
                break;
            case 9:
                transform.eulerAngles = new Vector3 { x = 0, y = 120, z = 0 };
                break;
            case 10:
                transform.eulerAngles = new Vector3 { x = 0, y = 180, z = 0 };
                break;
            case 1:
                transform.eulerAngles = new Vector3 { x = 0, y = 240, z = 0 };
                break;
            case -9:
                transform.eulerAngles = new Vector3 { x = 0, y = 300, z = 0 };
                break;
            default:
                break;
        }
    }

    public void stepBack()
    {
        moveQueue.Insert(0, new int[] { next_x, next_y });
        scheduledNormalMovement = false; // remove attempted move
    }

    public void showPathFeedback()
    {
        foreach (int[] location in moveQueue)
        {
            currentTile = gameController.GetComponent<BoardController>().getTile(new Vector2(location[0], location[1])); // Get the tile
            currentTile.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true; // Enable the ring
        }
    }

    public void hidePathFeedback()
    {
        foreach (int[] location in moveQueue)
        {
            currentTile = gameController.GetComponent<BoardController>().getTile(new Vector2(location[0], location[1])); // Get the tile
            currentTile.gameObject.transform.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false; // Disable the ring
        }
    }

    public void IsSelected() {
        transform.GetChild(1).GetComponent<Renderer>().enabled = true;}

    public void removeSelectionFeedback() {
        transform.GetChild(1).GetComponent<Renderer>().enabled = false;}

    public int getPlayerID() {
        return playerID; }

    public void setPlayerID(int newID) {
        playerID = newID; }

    public int getTeamID() {
        return teamID; }

    public void setTeamID(int newID) {
        teamID = newID; }

    public void set(int new_x, int new_y)
    {

        x = new_x;
        y = new_y;
        next_x = x;
        next_y = y;
    }

    public int getPriority() {
        return priority; }

    public int[] getCurrentPosition()
    {
        int[] ans = { x, y };
        return ans;
    }

    public int getX() {
        return x;}

    public int getY() {
        return y;}

    public float getHealth() { 
        return health;}

    public void reduceHealth(float damageTaken)
    {
        health = health - damageTaken;
        Debug.Log(health);
        if (health <= 0)
        {
            health = 0;
        }
        // schedule a "took damage" animation
        scheduledHit = true;
    }

    public void killYourself()
    {
        //gameController.GetComponent<BoardController>().removeFromUnitList(gameObject);
        gameController.GetComponent<BoardController>().setUnit(x, y, null);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Destroy(transform.GetChild(0).gameObject);

        hidePathFeedback();
        moveQueue = null;
        goalCoordinates = null;
        //currentTile.GetComponent<MeshRenderer>().material = materialNotSelected; // Change the material of the tile
        attackTarget = null;
        hasDied = true;

    }
}
