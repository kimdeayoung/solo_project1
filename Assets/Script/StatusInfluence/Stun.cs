using UnityEngine;

public class Stun : StatusInfluence
{
    public override StatusInfluenceType InfluenceType => StatusInfluenceType.Stun;

    public override void OnStart(WorldObject unit, WorldObject caster, AddStatusInfluenceData data)
    {
        base.OnStart(unit, caster, data);
        unit.Status.IncreaseStunCount();
    }

    protected override void OnEnd()
    {
        owner.Status.IncreaseStunCount();
    }
}
