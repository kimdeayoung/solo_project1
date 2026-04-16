using UnityEngine;

public class Stun : StatusInfluence
{
    public override StatusInfluenceType InfluenceType => StatusInfluenceType.Stun;

    public override void OnStart(WorldObject unit, AddStatusInfluenceData data)
    {
        base.OnStart(unit, data);
        unit.Status.IncreaseStunCount();
    }

    protected override void OnEnd()
    {
        owner.Status.IncreaseStunCount();
    }
}
