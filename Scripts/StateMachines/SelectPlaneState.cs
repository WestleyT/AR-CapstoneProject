using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class SelectPlaneState : BattleState {

    //we will select the plane, and generate the game map here

    bool aPlaneIsSelected = false;

    

    public override void Enter() {
        base.Enter();
        _planeExtractor.SetActive(true);
        UpdateHUD("Point controller", "at desired table.");
    }

    private void Update() {
        if (hit.transform != null) {
            if (hit.transform.gameObject.CompareTag("planePrefab")) {
                UpdateHUD("Press trigger to", "select blue surface.");
                Renderer renderer = hit.transform.gameObject.GetComponent<Renderer>();
                renderer.material.color = Color.blue;


            }
        }
    }

    protected override void TriggerPressed(byte controller_id, float triggerDownThreshold) {
        base.TriggerPressed(controller_id, triggerDownThreshold);

        if (hit.transform.CompareTag("planePrefab") && aPlaneIsSelected == false) {
            MapGenerator(hit);
            aPlaneIsSelected = true;
        }
    }

    //generate map here
    private void MapGenerator(RaycastHit selectedPlane) {
        GameObject thePlane = selectedPlane.transform.gameObject;
        MeshGenerator meshGenerator = _battleMap.GetComponent<MeshGenerator>();

        //find the ratio of the selected plane's x and y sides
        //remember, the quad's dimensions are in X/Y and the mesh's are in
        //X/Z, so we are feeding a y value into the mesh's Z value
        float theRatio = thePlane.transform.localScale.x / thePlane.transform.localScale.y;
        int baseSize = meshGenerator.squareMeshVerticeBaseSize;

        float xSize = theRatio * baseSize;
        int xSizeInt = (int)xSize;

        //set map's position
        _battleMap.transform.position = thePlane.transform.position;

        //set map center's rotation
        _battleMap.transform.rotation = thePlane.transform.rotation;
        _battleMap.transform.rotation *= Quaternion.Euler(-90, 0, 0);

        //scale the map
        //theScale = 1 / baseSize;
        _battleMap.transform.localScale = new Vector3(thePlane.transform.localScale.x / xSize, _battleMap.transform.localScale.y / baseSize, thePlane.transform.localScale.y / baseSize);

        //send it to _battleMap's generate mesh function
        meshGenerator.createShape(xSizeInt, baseSize);

        //kill the planes, then turn the plane selector off
        _planeExtractor.GetComponent<PlaneExtractor>().DestroyOldPlanes();
        _planeExtractor.SetActive(false);

        //Go To Spawn Unit State
        owner.ChangeState<SpawnUnitState>();
    }

    private void GenerateTerrainTypes() {
        //do terrain types here
    }

    public override void Exit() {
        base.Exit();
        _planeExtractor.SetActive(false);
    }
}
