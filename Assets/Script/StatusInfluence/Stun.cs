using UnityEngine;

public class Stun : StatusInfluence
{
    public override StatusInfluenceType InfluenceType => StatusInfluenceType.Stun;

    public override void OnStart(BattleUnit unit)
    {
        base.OnStart(unit);
        unit.Status.IncreaseStunCount();
    }

    protected override void OnEnd()
    {
        unit.Status.IncreaseStunCount();
    }
    
    public override bool OnUpdate(float deltaTime)
    {
        Duration -= deltaTime;
        if (Duration < 0)
        {
            OnEnd();
            return true;
        }
        return false;
    }
}
