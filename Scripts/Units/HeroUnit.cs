using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class HeroUnit : MonoBehaviour {

    [SerializeField]
    private HeroData _heroData;

    public bool turnFinished = false;
    public bool hasMoved = false;
    public bool beingLookedAt = false;
    public bool isCurrentUnit = false;

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
    [SerializeField]
    private Image hBar;
    public Image background;

    private NavMeshAgent agent;

    private GameObject body;

    //animation stuff
    private Animator anim;
    int attackHash = Animator.StringToHash("isShooting");

    //unit stats
    public float currentHP;
    public float attackDamage;

    private float hBarRatio;

    //access the info from the Scriptable Object throughout this script when needed
    //currently debugging to start just as an example
    private void Awake() {
        //setting agent
        agent = GetComponent<NavMeshAgent>();

        //disable the temp body and set the real one. Also get animator
        transform.GetChild(0).gameObject.SetActive(false);
        body = Instantiate(_heroData.BodyModel, transform.position, transform.rotation, transform);
        anim = GetComponentInChildren<Animator>();

        //HP shenanigans 
        currentHP = _heroData.MaxHP;
        hBarRatio = 1 / currentHP;

        //setting UI element variables
        _mainCamera = Camera.main;
        nameText.text = _heroData.HeroName;
        classText.text = _heroData.UnitClass;

        //SpriteRenderer rend = background.GetComponent<SpriteRenderer>();
        //background.color = new Color(66, 134, 244, 255); //blue
        background.color = Color.blue;

        //setting initial stats
        currentHP = _heroData.MaxHP;
        attackDamage = _heroData.AttackPower;
    }

    private void Update() {
        StayGrounded();

        //changing tag color if not being pointed to 
        ChangeTagColor();
    }

    //billboarding the unit's worldspace UI elements
    private void LateUpdate() {
        unitCanvas.transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward, Vector3.up);

        //chainging tag color if being looked at
        if (beingLookedAt == true && isCurrentUnit == false && turnFinished == false) {
            background.color = Color.red;
            beingLookedAt = false;
        }
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

    //public IEnumerator Move(Vector3 locationTo) {
    //    Vector3 startPos = transform.position;
    //    while (transform.position != locationTo) {
    //        transform.position = Vector3.MoveTowards(transform.position, locationTo, 0.5f * Time.deltaTime);
    //        yield return new WaitForEndOfFrame();
    //    }

    //    yield return new WaitForSeconds(1.25f);
    //    hasMoved = true;
    //}

    public void Move(Vector3 locationTo) {
        agent.SetDestination(locationTo);
        agent.isStopped = false;

        hasMoved = true;
    }

    public void Attack(GameObject target) {
        EnemyUnit enemyUnit = target.GetComponent<EnemyUnit>();

        //anim.SetTrigger(attackHash);
        anim.SetBool(attackHash, true);

        enemyUnit.currentHP -= attackDamage;

        anim.SetBool(attackHash, false);
    }

    private void UpdateHealthBar() {
        hBar.fillAmount = currentHP * hBarRatio;
    }

    private void ChangeTagColor() {
        if (turnFinished) {
            background.color = Color.gray;
        } else if (isCurrentUnit) {
            background.color = Color.yellow;
        } else {
            background.color = Color.blue;
        }

        //else if (beingLookedAt == false) {
        //    background.color = Color.blue;
        //}
    }
}
