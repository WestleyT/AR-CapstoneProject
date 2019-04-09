using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitState : BattleState {

    float MapZDimensions = 20; //find a way to pull this from MeshGenerator later

    float unitScaleModifier = 0.4f;

    int EnemyCountInitial = 5;

    MeshGenerator meshGenerator;
    Matrix4x4 mapMatrix;
    Mesh mesh;

    private int rowNumb;
    private int firstSpawnPoint;
	
	void Start () {
        meshGenerator = _battleMap.GetComponent<MeshGenerator>();
        mapMatrix = meshGenerator.mapPointMatrix;
        mesh = _battleMap.GetComponent<MeshFilter>().mesh;

        rowNumb = mesh.vertices.Length / 21; //divide total number of verts by 21 to find number of rows
        firstSpawnPoint = rowNumb + (rowNumb / 5); //1 full row + 1/5 of a row is the vertice to start spawning the first hero unit

        //find the scale
        float scale = _battleMap.transform.localScale.x;

        //spawn the Hero Units
        for (int i = 0; i < _heroUnits.Length; i++) {

            //set the unit's parent to the battleMap so it scales
            _heroUnits[i].transform.SetParent(_battleMap.transform);

            //set the unit's starting position and scale
            _heroUnits[i].transform.localScale = _heroUnits[i].transform.localScale * scale * unitScaleModifier;
            _heroUnits[i].transform.rotation = _battleMap.transform.rotation;

            //spawning units based on map's mesh verticies
            int spawnSpot = i + firstSpawnPoint + ((i + 1) * 2); //we offset each new Hero Unit by 2 with the ((i + 1) * 2) bit 
            _heroUnits[i].transform.position = mapMatrix.MultiplyPoint3x4(mesh.vertices[spawnSpot]);

            _heroUnits[i].SetActive(true);
        }

        //spawn enemy units and add them to list
        for (int i = 0; i < EnemyCountInitial; i++) {
            int enemySpawnStart = (rowNumb * 18) + (rowNumb / 5);
            int enemySpawnPoint = i + enemySpawnStart + ((i + 1) * 2);
            _enemyUnitsList.Add(Instantiate(_enemyUnitTypes[0], mapMatrix.MultiplyPoint3x4(mesh.vertices[enemySpawnPoint]), _battleMap.transform.rotation * Quaternion.Euler(0, 180, 0), _battleMap.transform));
        }

        //scale enemy units
        for (int i = 0; i < _enemyUnitsList.Count; i++) {
            _enemyUnitsList[i].transform.localScale = _heroUnits[0].transform.localScale;
        }

        //spawn objective

        Objective.SetActive(true);
        Objective.transform.localScale *= (scale * unitScaleModifier);
        Objective.transform.position = mapMatrix.MultiplyPoint3x4(mesh.vertices[(rowNumb * 19) + (rowNumb / 2)]);

        //set scale for the different cursors
        _moveCursor.transform.localScale *= (scale * unitScaleModifier);
        _attackCursor.transform.localScale *= (scale * unitScaleModifier);

        owner.ChangeState<SelectUnitState>();
	}
	
	
}
