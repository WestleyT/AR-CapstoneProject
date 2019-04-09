using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data", order = 52)]
public class EnemyData : ScriptableObject {

    [SerializeField]
    private string enemyName;

    [SerializeField]
    private string enemySpecies;

    [SerializeField]
    private float maxHP;

    [SerializeField]
    private float baseMoveSpeed;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private float attackDamage;

    [SerializeField]
    private GameObject enemyBody;

    public string EnemyName { get { return enemyName; } }

    public string EnemySpecies { get { return enemySpecies; } }

    public float MaxHP { get { return maxHP; } }

    public float BaseMoveSpeed { get { return baseMoveSpeed; } }

    public float AttackRange { get { return attackRange; } }

    public float AttackDamage { get { return attackDamage; } }

    public GameObject EnemyBody { get { return enemyBody; } }
}
