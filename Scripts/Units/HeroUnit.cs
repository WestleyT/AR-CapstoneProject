using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUnit : MonoBehaviour {

    [SerializeField]
    private HeroData _heroData;

    public bool turnFinished = false;
    public bool hasMoved = false;

    public Material baseMaterial;
    public Material turnOverMaterial;

    //variables for the UI elements
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text classText;
    [SerializeField]
    private Canvas unitCanvas;
    private Camera _mainCamera;

    //unit stats
    public float currentHP;
    public float attackDamage;

    //access the info from the Scriptable Object throughout this script when needed
    //currently debugging to start just as an example
    private void Start() {
        //setting UI element variables
        _mainCamera = Camera.main;
        nameText.text = _heroData.HeroName;
        classText.text = _heroData.UnitClass;

        //setting initial stats
        currentHP = _heroData.MaxHP;
        attackDamage = _heroData.AttackPower;
    }

    private void Update() {
        StayGrounded();
    }

    //billboarding the unit's worldspace UI elements
    private void LateUpdate() {
        unitCanvas.transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward, Vector3.up);
    }

    private void StayGrounded() {
        RaycastHit downHit;
        if (Physics.Raycast(transform.position, Vector3.down, out downHit)) {
            if (downHit.transform.gameObject.CompareTag("theMap")) {
                float distanceToGround = downHit.distance;
                Vector3 currentPos = transform.position;
                float newY = currentPos.y - distanceToGround;
                transform.position = new Vector3(currentPos.x, newY, currentPos.z);
            }
        }
    }

    public IEnumerator Move(Vector3 locationTo) {
        Vector3 startPos = transform.position;
        while (transform.position != locationTo) {
            transform.position = Vector3.MoveTowards(transform.position, locationTo, 0.5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.25f);
        hasMoved = true;
    }
}
