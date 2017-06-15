using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Archer : UnitController {
    private float attackRange;

    protected override void setUnitParameters() {
        health = 60;
        attack = 10;
        attackRange = 6;
        return;
    }

    /*public override void executeNextAttack()
    {
        // if has target
        if (attackTarget == null)
            return;
        else
        {
            // Check if can het target, if yes, attack

            int diffx = goalCoordinates[0] - x;
            int diffy = goalCoordinates[1] - y;

            if (hexMath.hexDistance(diffx, diffy) < attackRange)
            {
                // ATTACK
                attackTarget.GetComponent<UnitController>().reduceHealth((float)attack);
            }
                

        }
    }*/

}
