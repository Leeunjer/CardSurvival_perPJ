using UnityEngine;

public class AggressiveAI : EnemyJudgmentData
{
    public override Think Judgment(int Attackstack, int GuardStack, int Healstack)
    {
        // 최대 스택이 같으면 공격, 방어, 회복 순으로 우선한다.
        int attackChance;
        if (Attackstack >= GuardStack && Attackstack >= Healstack)
            attackChance = 50;
        else if (GuardStack >= Healstack)
            attackChance = 70;
        else
            attackChance = 100;

        return Random.Range(0, 100) < attackChance ? Think.Attack : Think.Guard;
    }
}
