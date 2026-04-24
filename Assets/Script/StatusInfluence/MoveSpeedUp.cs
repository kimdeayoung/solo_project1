using UnityEngine;

public class MoveSpeedUp : StatusInfluence
{
    private float applyValue;
    public override StatusInfluenceType InfluenceType => StatusInfluenceType.MoveSpeedUp;

    public override void OnStart(WorldObject unit, WorldObject caster, AddStatusInfluenceData data)
    {
        base.OnStart(unit, caster, data);

        applyValue = data.value;
        owner.Status.ChangeMoveSpeed(applyValue);
    }

    public override void AddInfluence(WorldObject caster, AddStatusInfluenceData data)
    {
        base.AddInfluence(caster, data);

        applyValue += data.value;
        owner.Status.ChangeMoveSpeed(data.value);
    }

    protected override void OnEnd()
    {
        owner.Status.ChangeMoveSpeed(-applyValue);
    }
}
