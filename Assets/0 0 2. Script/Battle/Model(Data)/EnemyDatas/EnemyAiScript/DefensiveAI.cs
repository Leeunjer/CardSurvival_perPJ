using UnityEngine;

public class DefensiveAI : EnemyJudgmentData
{
    public override Think Judgment(int Attackstack, int GuardStack, int Healstack)
    {
        int num = Random.Range(0, 100);
        // 최대 스택이 같으면 공격, 방어, 회복 순으로 우선한다.
        int attackChance;
        if (Attackstack >= GuardStack && Attackstack >= Healstack)
            attackChance = 0;
        else if (GuardStack >= Healstack)
            attackChance = 50;
        else
            attackChance = 70;

        return num < attackChance ? Think.Attack : num % 2 == 0 ? Think.Guard : Think.Heal;
    }
}
