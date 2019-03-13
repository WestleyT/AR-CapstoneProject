using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitState : BattleState {

    float MapZDimensions = 20; //find a way to pull this from MeshGenerator later

    float unitScaleModifier = 0.4f;

    int EnemyCountInitial = 5;
	
	void Start () {
        //find the scale
        float scale = _battleMap.transform.localScale.x;

        //spawn the Hero Units
		for (int i = 0; i < _heroUnits.Length; i++) {

            //set the unit's parent to the battleMap so it scales
            _heroUnits[i].transform.SetParent(_battleMap.transform);

            //set the unit's starting position and scale
            _heroUnits[i].transform.localScale = _heroUnits[i].transform.localScale * scale * unitScaleModifier;
            _heroUnits[i].transform.rotation = _battleMap.transform.rotation;
            _heroUnits[i].transform.localPosition = _battleMap.transform.position + Vector3.forward * -1 * (MapZDimensions * 0.4f);
            _heroUnits[i].transform.localPosition += new Vector3(i * 1.5f - 2, 0, 0);
            _heroUnits[i].transform.localPosition += new Vector3(0, 1, 0);
            
            _heroUnits[i].SetActive(true);
        }

        //spawn enemy units and add them to list
        for (int i = 0; i < EnemyCountInitial; i++) {
            _enemyUnitsList.Add(Instantiate(_enemyUnitTypes[0], _battleMap.transform.position, _battleMap.transform.rotation, _battleMap.transform));
        } // + Vector3.forward * (MapZDimensions * 0.4f)
        //scale and position enemy units
        for (int i = 0; i < _enemyUnitsList.Count; i++) {
            _enemyUnitsList[i].transform.localScale = _heroUnits[0].transform.localScale;
            _enemyUnitsList[i].transform.localPosition += new Vector3(i * 1.5f - 2, 1, transform.localPosition.z * (MapZDimensions * 0.4f));
        }

        //set scale for the different cursors
        _moveCursor.transform.localScale *= (scale * unitScaleModifier);
        _attackCursor.transform.localScale *= (scale * unitScaleModifier);

        owner.ChangeState<SelectUnitState>();
	}
	
	
}
