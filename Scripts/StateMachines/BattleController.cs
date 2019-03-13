using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.UI;

public class BattleController : StateMachine {

    //This is a State Machine attached to the BattleController game object.
    //It houses references that will be useful to the states.
    public GameObject planeExtractor;
    public GameObject controllerBeam;
    public GameObject battleMap;
    public GameObject _theCamera;
    public GameObject[] heroUnits;
    public List<GameObject> enemyUnits;
    public HeroUnit currentUnit;
    public GameObject[] enemyUnitTypes;
    public GameObject moveCursor;
    public GameObject attackCursor;

    public Text TopHUDText;
    public Text BottomHUDText;
    
    //ML Controller Data
    public MLInputController controller;
    public Vector3 controllerForward;
    public Vector3 controllerPosition;
    public RaycastHit hit;
    public GameObject controllerUI;

    private void Start() {
        MLInput.Start();
        controller = MLInput.GetController(MLInput.Hand.Left);

        ChangeState<SelectPlaneState>();
    }

    private void OnDestroy() {
        MLInput.Stop();
    }

    private void Update() {
        UpdateControllerData();
        if (Physics.Raycast(controllerPosition, controllerForward, out hit)) {
            //stuff here?
            
        }
    }

    public virtual void UpdateControllerData() {
        //positioning the controller beam
        controllerBeam.transform.position = controller.Position;
        controllerBeam.transform.rotation = controller.Orientation;

        //setting forward rotation for raycasts
        controllerForward = controller.Orientation * Vector3.forward;
        controllerPosition = controller.Position;

        //positioning ControllerUI
        controllerUI.transform.position = controller.Position;
        controllerUI.transform.rotation = controller.Orientation * Quaternion.Euler(60, 0, 0);
    }
}
