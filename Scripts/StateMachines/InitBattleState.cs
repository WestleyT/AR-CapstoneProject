using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBattleState : BattleState {

    //When we first enter a battle
    //perhaps select table/generate map in here

    public override void Enter() {
        base.Enter();
        StartCoroutine(Init());
    }

    IEnumerator Init() {
        //do things like load map data
        yield return null;
        owner.ChangeState<SelectUnitState>();
    }
}
