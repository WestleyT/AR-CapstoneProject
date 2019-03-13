using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour {

    [SerializeField]
    private EnemyData _enemyData;

    //unit's stats
    public float currentHP;

	// Use this for initialization
	void Start () {
        currentHP = _enemyData.MaxHP;
	}
	
	// Update is called once per frame
	void Update () {
        StayGrounded();
	}

    public void DoBehavior() {
        Debug.Log("Doing " + gameObject.name + "'s action");

        StartCoroutine("Movement");
    }

    private IEnumerator Movement() {
        //move randomly for now
        //Vector3 newLocation = new Vector3(Random.Range(0, 1), 0.01f, Random.Range(0, 1));
        Vector3 newLocation = transform.position + new Vector3(Random.Range(0, 5), 0, Random.Range(0, 5));

        while (transform.position != newLocation) {
            transform.position = Vector3.MoveTowards(transform.position, newLocation, 0.5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
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
}
