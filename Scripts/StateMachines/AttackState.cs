using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class AttackState : BattleState {

    public override void Enter() {
        base.Enter();
        _attackCursor.SetActive(true);
    }

    private void Update() {
        if (hit.transform != null) {
            if (hit.transform.gameObject.CompareTag("EnemyUnit")) {
                _attackCursor.transform.position = hit.transform.position;
            }
            else {
                _attackCursor.transform.position = hit.point;
            }
        }
    }

    //selecting unit to attack, need a within range function
    protected override void TriggerPressed(byte controller_id, float triggerDownThreshold) {
        base.TriggerPressed(controller_id, triggerDownThreshold);

        if (hit.transform.gameObject.CompareTag("EnemyUnit")) { //&& within range
            EnemyUnit enemyUnit = hit.transform.gameObject.GetComponent<EnemyUnit>();

            enemyUnit.currentHP -= currentUnit.attackDamage;

            //kill enemy if hp is below zero 
            if (enemyUnit.currentHP <= 0) {
                //update list to reflect killed enemy being gone
                _enemyUnitsList.Remove(enemyUnit.gameObject);

                Destroy(enemyUnit.gameObject); //add death animation later
            }

            EndUnitsTurn();
        }
    }

    //touch pad commands
    protected override void TouchPadEnd(byte controller_id, MLInputControllerTouchpadGesture gesture) {
        base.TouchPadEnd(controller_id, gesture);

        //end unit's turn
        if (gesture.Direction == MLInputControllerTouchpadGestureDirection.Down) {
            EndUnitsTurn();
        }
    }

    public override void Exit() {
        base.Exit();
        _attackCursor.SetActive(false);
    }
}
