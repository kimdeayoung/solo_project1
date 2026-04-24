using UnityEngine;

public class Knockback : StatusInfluence
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float initialDuration;

    public override StatusInfluenceType InfluenceType => StatusInfluenceType.Knockback;

    public override void OnStart(WorldObject unit, WorldObject caster, AddStatusInfluenceData data)
    {
        base.OnStart(unit, caster, data);

        Vector3 direction = (owner.transform.position - caster.transform.position).normalized;

        startPosition = owner.transform.position;
        targetPosition = direction * data.value;
        initialDuration = data.duration;
        owner.Status.SetKnockbackState(true);
    }

    public override bool OnUpdate(float deltaTime)
    {
        bool returnValue = base.OnUpdate(deltaTime);
        if (!returnValue)
        {
            owner.Rigidbody.MovePosition(Vector3.Lerp(startPosition, targetPosition, 1f - (Duration / initialDuration)));
        }
        return returnValue;
    }

    public override void AddInfluence(WorldObject caster, AddStatusInfluenceData data)
    {
        Debug.Assert(false);
    }

    protected override void OnEnd()
    {
        owner.Status.SetKnockbackState(false);
    }
}
