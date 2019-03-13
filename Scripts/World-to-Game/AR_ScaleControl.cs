using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(Camera))]
public class AR_ScaleControl : MonoBehaviour {

    //This component is attached to the camera and updates the scale of its empty parent
    //and all other device-generated game-objects, attached to said parent. To properly
    //scale in AR, the scale of this "root object" should be altered inversly to
    //the desired scale of the generated game-objects. This is the current
    //best practice, as it reduces scale-related physics errors.
    [SerializeField]
    public float _scale;

    private Camera _camera;
    private float OGnearClipDistance;
    private float OGfarClipDistance;
    private Transform NodePos;

    public float Scale { get { return _scale; } }

    private void Start() {
        _camera = GetComponent<Camera>();
        OGnearClipDistance = Camera.main.nearClipPlane;
        OGfarClipDistance = Camera.main.farClipPlane;
        NodePos = this.GetComponentInParent<Transform>();
        //UpdateWorldScale();
    }

    private void Update() {
        UpdateWorldScale();
    }

    private void UpdateWorldScale() {
        //update parent node's scale
        _camera.transform.parent.localScale = new Vector3(_scale, _scale, _scale);

        //calculate new clip distance
        float worldScale = _camera.transform.parent.lossyScale.x;

        //update clipping planes
        _camera.nearClipPlane = OGnearClipDistance * worldScale;
        _camera.farClipPlane = OGfarClipDistance * worldScale;

        //tell the headset the scale has changed
        MagicLeapDevice.UpdateWorldScale();
    }

    private void UpdateMapDistance() {

    }


}
