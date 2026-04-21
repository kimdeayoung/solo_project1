using UnityEngine;

public class EnemyStatus : UnitStatus
{
    public EnemyStatus(BattleUnit owner, UnitStat stat) : base(owner, stat)
    {
    }

    public override void ChangeMoveSpeed(float value)
    {
        base.ChangeMoveSpeed(value);

        Enemy enemy = owner as Enemy;
        Debug.Assert(enemy != null);

        enemy.ResetAgentProperty(new NavMeshAgentProperty(StatusAttributes.MoveSpeed, StatusAttributes.RotateSpeed));
    }
}
