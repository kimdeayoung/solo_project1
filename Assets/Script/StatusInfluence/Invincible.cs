using UnityEngine;

public class Invincible : StatusInfluence, IHitStatusInfluence
{
    private int damageNullifyCount;

    public override StatusInfluenceType InfluenceType => StatusInfluenceType.Invincible;

    public override RemoveStatusInfluenceType RemoveStatusInfluenceType => RemoveStatusInfluenceType.All;

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

        HitParameter parameter = new HitParameter(hitParameter.atk, hitParameter.defT0, hitParameter.defT1, 0.0f, hitParameter.accelerationRatio, hitParameter.weightOffset);
        hitParameter = parameter;
    }

    protected override void OnEnd()
    {

    }
}
