using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy Data",menuName = "Scriptable Object/Enemy Data",order = int.MaxValue)]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    string enemyType;
    public string EnemyType { get { return enemyType; }}

    [SerializeField]
    int eHP;
    public int EHP { get { return eHP; }}

    [SerializeField]
    int damage;
    public int Damage { get { return damage; }}

    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; }}

    [SerializeField]
    float rotationSpeed;
    public float RotationSpeed { get { return rotationSpeed; }}

    [SerializeField]
    float targetRadius;
    public float TargetRadius { get { return targetRadius; }}

    [SerializeField]
    float targetRange;
    public float TargetRange { get { return targetRange; }}
}
