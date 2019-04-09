using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour {

    [SerializeField]
    private EnemyData _enemyData;

    private GameObject body;

    //variables for the UI elements
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text classText;
    [SerializeField]
    private Canvas unitCanvas;
    private Camera _mainCamera;
    public Image healthBar;
    private float healthRatio;

    //unit's stats
    public float currentHP;
    private float damagePower;

	// Use this for initialization
	void Awake () {
        transform.GetChild(0).gameObject.SetActive(false);
        body = Instantiate(_enemyData.EnemyBody, transform.position, transform.rotation, transform);

        damagePower = _enemyData.AttackDamage;

        currentHP = _enemyData.MaxHP;
        healthRatio = 1 / _enemyData.MaxHP;

        //setting UI element variables
        _mainCamera = Camera.main;
        nameText.text = _enemyData.EnemyName;
        classText.text = _enemyData.EnemySpecies;
        
    }
	
	// Update is called once per frame
	void Update () {
        StayGrounded();
        UpdateHealthBar();

        if (currentHP <= 0) {
            gameObject.SetActive(false);
        }
	}

    private void LateUpdate() {
        unitCanvas.transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward, Vector3.up);
    }

    public void DoBehavior() {
        Debug.Log("Doing " + gameObject.name + "'s action");

        StartCoroutine("Movement");
    }

    private IEnumerator Movement() {
        //Find closest hero unit, then move towards it a distances based off "movement speed" stat
        GameObject closestUnit = FindClosestTarget(GameObject.Find("BattleController").GetComponent<BattleController>());
        Vector3 newLocation = closestUnit.transform.position / (1/_enemyData.BaseMoveSpeed);

        while (transform.position != newLocation) {
            transform.position = Vector3.MoveTowards(transform.position, newLocation, 0.5f * Time.deltaTime);
            //yield return new WaitForEndOfFrame();
        }

        if (Vector3.Distance(transform.position, closestUnit.transform.position) < 0.2f) {
            AttackHero(closestUnit);
        }

        yield return new WaitForEndOfFrame();
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

    private void UpdateHealthBar() {
        healthBar.fillAmount = currentHP * healthRatio;
    }

    GameObject FindClosestTarget(BattleController battleController) {
        float tempDistance = 1000f;
        GameObject ClosestUnit = null;

        for (int i = 0; i < battleController.heroUnits.Length; ++i) {
            float currentDist = Vector3.Distance(transform.position, battleController.heroUnits[i].transform.position);

            if (ClosestUnit == null) {
                ClosestUnit = battleController.heroUnits[i];
                tempDistance = currentDist;
            } else if (currentDist < tempDistance) {
                ClosestUnit = battleController.heroUnits[i];
                tempDistance = currentDist;
            }
        }

        return ClosestUnit;
    }

    void AttackHero(GameObject targetUnit) {
        //do an animation

        //subtract the health
        targetUnit.GetComponent<HeroUnit>().currentHP -= damagePower;
    }
}
