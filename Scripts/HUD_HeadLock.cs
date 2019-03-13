using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_HeadLock : MonoBehaviour {

    public GameObject theCamera;

    [SerializeField]
    private const float distance = 2.0f;
    [SerializeField]
    private float speed = 1.75f;

    private void LateUpdate() {
        softHeadlock(speed);
    }

    private void softHeadlock(float speed) {
        speed *= Time.deltaTime;
        Vector3 posTo = theCamera.transform.position + (theCamera.transform.forward * distance);
        transform.position = Vector3.SlerpUnclamped(transform.position, posTo, speed);

        //Quaternion rotTo = theCamera.transform.rotation;
        Quaternion rotTo = Quaternion.LookRotation(transform.position - theCamera.transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotTo, speed);
    }
}
