using System;
using UnityEngine;



[Serializable]
public class EliteEnemyItem
{
    public GameObject EliteEnemyObjct;
    public int Hp;

    public int AttackDamage;
}


[CreateAssetMenu(fileName = "EliteEnemyData", menuName = "Scriptable Objects/EliteEnemyData")]
public class EliteEnemyData : ScriptableObject
{
    public EliteEnemyItem EliteEnemy;
}
