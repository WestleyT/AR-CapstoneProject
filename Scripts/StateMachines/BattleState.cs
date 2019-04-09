using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.UI;

public class BattleState : State {

    //This is the grandaddy class for all the states
    //used by BattleController. It holds references
    //to things BattleController references. BattleController
    //is referred to as "owner" in this and its subclasses.

    protected BattleController owner;

    public GameObject _planeExtractor { get { return owner.planeExtractor; } }
    public GameObject _controllerBeam { get { return owner.controllerBeam; } }
    public GameObject _battleMap { get { return owner.battleMap; } }
    public GameObject _theCamera { get { return owner._theCamera; } }
    public GameObject[] _heroUnits { get { return owner.heroUnits; } }
    public HeroUnit currentUnit { get { return owner.currentUnit; } }
    public GameObject _moveCursor { get { return owner.moveCursor; } }
    public GameObject _attackCursor { get { return owner.attackCursor; } }
    public List<GameObject> _enemyUnitsList { get { return owner.enemyUnits; } }
    public GameObject[] _enemyUnitTypes { get { return owner.enemyUnitTypes; } }
    public GameObject _roughTerrain { get { return owner.roughTerrain; } }
    public GameObject Objective { get { return owner.objective; } }

    public Vector3 controllerPosition { get { return owner.controllerPosition; } }
    public Vector3 controllerForward { get { return owner.controllerForward; } }
    //public bool TriggerIsPressed { get { return owner.TriggerIsPressed; } }
    public RaycastHit hit { get { return owner.hit; } }
    public MLInputController theController { get { return owner.controller; } }
    public Text topText { get { return owner.TopHUDText; } }
    public Text bottomText { get { return owner.BottomHUDText; } }

    
    //getting/handling delegates and whatnot from the ML Controller API
    protected virtual void Awake () {
        owner = GetComponent<BattleController>();
        MLInput.TriggerDownThreshold = 0.7f;
        MLInput.TriggerUpThreshold = 0.3f;
    }

    protected override void AddListeners() {
        base.AddListeners();
        MLInput.OnControllerButtonDown += OnButtonDown;
        MLInput.OnControllerButtonUp += OnButtonUp;
        MLInput.OnTriggerDown += TriggerPressed;
        MLInput.OnTriggerUp += TriggerReleased;
        MLInput.OnControllerTouchpadGestureEnd += TouchPadEnd;
    }

    protected override void RemoveListeners() {
        base.RemoveListeners();
        MLInput.OnControllerButtonDown -= OnButtonDown;
        MLInput.OnControllerButtonUp -= OnButtonUp;
        MLInput.OnTriggerDown -= TriggerPressed;
        MLInput.OnTriggerUp -= TriggerReleased;
        MLInput.OnControllerTouchpadGestureEnd -= TouchPadEnd;
    }

    protected virtual void OnButtonDown(byte controller_id, MLInputControllerButton button) {
        //called by child states
        //ex: if(button == MLInputControllerButton.Bumper) {do whatever};
    }

    protected virtual void OnButtonUp(byte controller_id, MLInputControllerButton button) {
        //called by child states
        //ex: if(button == MLInputControllerButton.Bumper) {do whatever};
    }

    protected virtual void TriggerPressed(byte controller_id, float triggerDownThreshold) {

    }

    protected virtual void TriggerReleased(byte controller_id, float triggerUpThreshold) {

    }

    protected virtual void TouchPadEnd (byte controller_id, MLInputControllerTouchpadGesture gesture) {

    }

    //adding UpdateHUD method that sets HUD text to blank to Exit method for all subclasses
    public override void Exit() {
        base.Exit();
        UpdateHUD(" ", " ");
    }

    protected void EndUnitsTurn() {
        currentUnit.turnFinished = true;
        currentUnit.gameObject.GetComponent<HeroUnit>().background.color = Color.grey; //grey
        owner.currentUnit = null;
        owner.ChangeState<SelectUnitState>();
    }

    protected void UpdateHUD(string TopWords, string BottomWords) {
        owner.TopHUDText.text = TopWords;
        owner.BottomHUDText.text = BottomWords;
    }
}
