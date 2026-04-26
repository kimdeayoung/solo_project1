using UnityEngine;

public class Invincible : StatusInfluence, IHitStatusInfluence
{
    private int damageNullifyCount;

    public override StatusInfluenceType InfluenceType => StatusInfluenceType.Invincible;

    public override void OnStart(WorldObject unit, WorldObject caster, AddStatusInfluenceData data)
    {
        base.OnStart(unit, caster, data);

        damageNullifyCount = (int)data.value;
    }

    public override void AddInfluence(WorldObject caster, AddStatusInfluenceData data)
    {
        base.AddInfluence(caster, data);

        damageNullifyCount += (int)data.value;
    }

    public void RunOnHitAction(ref HitParameter hitParameter)
    {
        if (damageNullifyCount > 0)
        {
            if (--damageNullifyCount == 0)
            {
                RemoveInfluence();
            }
        }

        HitParameter parameter = new HitParameter(hitParameter.statusAttributes, 0.0f);
        hitParameter = parameter;
    }

    protected override void OnEnd()
    {

    }
}
