using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero Data", menuName = "Hero Data", order = 51)]
public class HeroData : ScriptableObject {

    //so we can create this in the inspector
    [SerializeField]
    private string heroName;

    [SerializeField]
    private string unitClass;

    [SerializeField]
    private float maximumHP;

    [SerializeField]
    private float baseMovementSpeed;

    [SerializeField]
    private float attackPower;

    [SerializeField]
    private float weaponRange;

    [SerializeField]
    private GameObject bodyModel;

    //so we can access this info from other scripts

    public string HeroName { get { return heroName; } }

    public string UnitClass { get { return unitClass; } }

    public float MaxHP { get { return maximumHP; } }

    public float BaseMoveSpeed { get { return baseMovementSpeed; } }

    public float AttackPower { get { return attackPower; } }

    public float WeaponRange { get { return weaponRange; } }

    public GameObject BodyModel { get { return bodyModel; } }
}
