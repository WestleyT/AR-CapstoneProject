using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class SelectUnitState : BattleState {

    bool hasBumped = false;

    public override void Enter() {
        base.Enter();
        CheckAndChangeState();
        Debug.Log("IN unit select state");
    }

    public override void Exit() {
        base.Exit();
    }

    private void Update() {
        if (hit.transform != null) {
            //handling controller vibration on "contact" with friendly unit
            if (hit.transform.gameObject.CompareTag("HeroUnit") && hasBumped == false) {
                hasBumped = true;
                theController.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.Bump, MLInputControllerFeedbackIntensity.Medium);
            }
            if (!hit.transform.gameObject.CompareTag("HeroUnit") && hasBumped == true) {
                hasBumped = false;
            }

            //change unit's color if being pointed at 
            if (hit.transform.gameObject.CompareTag("HeroUnit")) {
                hit.transform.gameObject.GetComponent<HeroUnit>().beingLookedAt = true;
            }
        }
    }

    protected override void TriggerPressed(byte controller_id, float triggerDownThreshold) {
        base.TriggerPressed(controller_id, triggerDownThreshold);

        if(hit.transform.gameObject.CompareTag("HeroUnit") && hit.transform.gameObject.GetComponent<HeroUnit>().turnFinished == false) {
            owner.currentUnit = hit.transform.gameObject.GetComponent<HeroUnit>();
            currentUnit.isCurrentUnit = true;
        }
    }

    protected override void TouchPadEnd(byte controller_id, MLInputControllerTouchpadGesture gesture) {
        base.TouchPadEnd(controller_id, gesture);

        if (owner.currentUnit != null) {
            if (gesture.Direction == MLInputControllerTouchpadGestureDirection.Up) {
                owner.ChangeState<MoveState>();
            }

            if (gesture.Direction == MLInputControllerTouchpadGestureDirection.Right) {
                owner.ChangeState<AttackState>();
            }

            //make this wait ("end unit's turn") state for now
            if (gesture.Direction == MLInputControllerTouchpadGestureDirection.Down) {
                currentUnit.turnFinished = true;
                owner.currentUnit = null;
                CheckAndChangeState();
            }
        }
    }

    //check if all the hero units have ended turn. If yes, switch to enemy turn
    private void CheckAndChangeState() {
        int hasMovedCounter = 0;

        for (int i = 0; i < _heroUnits.Length; i++) {
            if (_heroUnits[i].GetComponent<HeroUnit>().turnFinished == true) {
                hasMovedCounter += 1;
                
                if (hasMovedCounter == _heroUnits.Length) {
                    owner.ChangeState<EnemyTurn>();
                }
            }
        } 
    }
}
