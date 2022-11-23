using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy Data",menuName = "Scriptable Object/Enemy Data",order = int.MaxValue)]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    int eHP;
    public int EHP { get { return eHP; }}

    [SerializeField]
    int eDamage;
    public int EDamage { get { return eDamage; }}

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
