using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class EnemyTurn : BattleState {

    public override void Enter() {
        base.Enter();
        StartCoroutine("ActivateEnemies");
    }

    public override void Exit() {
        base.Exit();
    }

    private IEnumerator ActivateEnemies() {
        //go through list of enemy units, and activate their move/attack behavior
        for (int i = 0; i < _enemyUnitsList.Count; i++) {
            _enemyUnitsList[i].GetComponent<EnemyUnit>().DoBehavior();
        }

        //once all the enemies have taken action, prepare the hero units and go to select unit state
        PrepForPlayerTurn();

        yield return new WaitForEndOfFrame();
    }

    void PrepForPlayerTurn() {
        //set all the flow-control bools to false again
        for (int i = 0; i < _heroUnits.Length; i++) {
            _heroUnits[i].GetComponent<HeroUnit>().hasMoved = false;
            _heroUnits[i].GetComponent<HeroUnit>().turnFinished = false;
            _heroUnits[i].GetComponentInChildren<Renderer>().material = _heroUnits[i].GetComponent<HeroUnit>().baseMaterial;
        }

        owner.ChangeState<SelectUnitState>();
    }
}
