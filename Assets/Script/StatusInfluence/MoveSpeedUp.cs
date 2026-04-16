using UnityEngine;

public class MoveSpeedUp : StatusInfluence
{
    private float applyValue;
    public override StatusInfluenceType InfluenceType => StatusInfluenceType.MoveSpeedUp;

    public override void OnStart(WorldObject unit, AddStatusInfluenceData data)
    {
        base.OnStart(unit, data);

        applyValue = data.value;
        owner.Status.ChangeMoveSpeed(applyValue);
    }

    public override void AddInfluence(AddStatusInfluenceData data)
    {
        base.AddInfluence(data);

        applyValue += data.value;
        owner.Status.ChangeMoveSpeed(data.value);
    }

    protected override void OnEnd()
    {
        owner.Status.ChangeMoveSpeed(-applyValue);
    }
}
