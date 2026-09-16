public abstract class EnemyJudgmentData
{
    public enum Think {Attack , Guard , Heal}
    public abstract Think Judgment(int Attackstack , int GuardStack , int Healstack);
}
