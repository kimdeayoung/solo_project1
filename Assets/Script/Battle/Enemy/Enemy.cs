using UnityEngine;

public class Enemy : BattleUnit
{
    [SerializeField] private EnemyStatusStat _stat;

    public override void Init(string assetName)
    {
        base.Init(assetName);

        Status = new EnemyStatus(this, _stat);
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override BattleUnitType Type => BattleUnitType.Enemy;

    public override void OnUpdate(float deltaTime)
    {
        BehaviourController.OnUpdate(deltaTime);
    }

    public override void OnFixedUpdate(float fixedDeltaTime)
    {
        BehaviourController.OnFixedUpdate(fixedDeltaTime);
    }
}
