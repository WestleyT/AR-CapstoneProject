using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class ControllerUIPos : MonoBehaviour {

    private MLInputController controller;

	// Use this for initialization
	void Start () {
        controller = MLInput.GetController(MLInput.Hand.Left);
    }
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = controller.Position;
        transform.rotation = controller.Orientation;
    }
}
