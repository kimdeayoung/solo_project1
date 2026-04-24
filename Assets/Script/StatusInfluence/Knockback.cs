using UnityEngine;

public class Knockback : StatusInfluence
{
    private Vector3 knockbackForce;

    public override StatusInfluenceType InfluenceType => StatusInfluenceType.Knockback;

    public override void OnStart(WorldObject unit, WorldObject caster, AddStatusInfluenceData data)
    {
        base.OnStart(unit, caster, data);

        Vector3 direction = (owner.transform.position - caster.transform.position).normalized;

        knockbackForce = direction * data.value;
        owner.Status.SetKnockbackState(true);
    }

    public override void OnFixedUpdate(float deltaTime)
    {
        base.OnFixedUpdate(deltaTime);
        owner.Rigidbody.AddForce(knockbackForce, ForceMode.VelocityChange);
    }

    public override void AddInfluence(WorldObject caster, AddStatusInfluenceData data)
    {
        Debug.Assert(false);
    }

    protected override void OnEnd()
    {
        owner.Rigidbody.linearVelocity = Vector3.zero;
        owner.Rigidbody.angularVelocity = Vector3.zero;

        owner.Status.SetKnockbackState(false);
    }
}
