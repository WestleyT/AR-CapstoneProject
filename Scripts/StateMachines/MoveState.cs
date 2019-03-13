using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class MoveState : BattleState {

    public override void Enter() {
        base.Enter();
        _moveCursor.SetActive(true);
    }

    private void Update() {
        //update move cursor's position
        if (hit.transform != null) {
            if (hit.transform.gameObject.CompareTag("HeroUnit")) {
                _moveCursor.transform.position = hit.transform.position;
            }
            else {
                _moveCursor.transform.position = hit.point;
            }
        }

        ////if a point on the map is selected with the trigger, send that as the currentUnit's moveTo position
        //if (TriggerIsPressed && hit.transform.gameObject.CompareTag("theMap")) {
        //    StartCoroutine(currentUnit.Move(hit.point));
        //}

        if (currentUnit != null) {
            if (currentUnit.hasMoved) {
                owner.ChangeState<AttackState>();
            }
        }
    }

    protected override void TriggerPressed(byte controller_id, float triggerDownThreshold) {
        base.TriggerPressed(controller_id, triggerDownThreshold);

        if (hit.transform.gameObject != null) {
            if (hit.transform.gameObject.CompareTag("theMap")) {
                StartCoroutine(currentUnit.Move(hit.point));
            }
        }
    }

    protected override void OnButtonDown(byte controller_id, MLInputControllerButton button) {
        base.OnButtonDown(controller_id, button);
        //if (button == MLInputControllerButton.)
    }

    protected override void TouchPadEnd(byte controller_id, MLInputControllerTouchpadGesture gesture) {
        base.TouchPadEnd(controller_id, gesture);

        if (gesture.Direction == MLInputControllerTouchpadGestureDirection.Down) {
            EndUnitsTurn();
        }
    }

    public override void Exit() {
        base.Exit();
        _moveCursor.SetActive(false);
    }
}
