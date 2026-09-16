using System;
using UnityEngine;

[Serializable]
public class EnemyItem
{
    
    public GameObject EnemyObject;
    public int Hp;

    public int AttackDamage;
    public int GuardDamage;
    
    public EnemyJudgmentData enemyJudgment;
    
}






[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public EnemyItem[] Enemys;
}
