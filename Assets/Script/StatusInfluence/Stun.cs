using UnityEngine;

public class Stun : StatusInfluence
{
    public override StatusInfluenceType InfluenceType => StatusInfluenceType.Stun;

    public override void OnStart(BattleUnit unit, AddStatusInfluenceData data)
    {
        base.OnStart(unit, data);
        unit.Status.IncreaseStunCount();
    }

    protected override void OnEnd()
    {
        owner.Status.IncreaseStunCount();
    }
}
