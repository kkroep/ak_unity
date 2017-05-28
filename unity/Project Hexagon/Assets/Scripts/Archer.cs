using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Archer : UnitController {
    private float attackRange;

    protected override void setUnitParameters() {
        health = 60;
        attack = 10;
        maxAP = 2;
        attackRange = 6;
        return;
    }

    public override void executeNextAttack(int AP_stage)
    {
        if (AP_stage > AP)
            return;
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
                attackTarget.GetComponent<UnitController>().reduceHealth((float)attack * (1+0.5f*(AP-1)));
                AP = 0;
            }
                

        }
    }

}
