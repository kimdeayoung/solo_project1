using System.Collections.Generic;
using UnityEngine;

public class MeleeHitParameter : ActionParameter
{
    private float damageMultiplier;
    private float hitThresholdAngle;

    public override ActionParameterType ActionParameterType => ActionParameterType.MeleeHit;

    public override void ResetVariables(ActionParameterSO data)
    {
        base.ResetVariables(data);
        MeleeHitParameterSO meleeHitParameter = data as MeleeHitParameterSO;
        Debug.Assert(meleeHitParameter != null);

        damageMultiplier = meleeHitParameter.DamageMultiplier;
        hitThresholdAngle = meleeHitParameter.HitThresholdAngle;
    }

    protected override void RunAction_Impl(WorldObject caster, List<WorldObject> targets)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            WorldObject target = targets[i];
            float dot = Vector3.Dot(caster.transform.forward, Vector3.Normalize(target.transform.position - caster.transform.position));
            if (dot < hitThresholdAngle)
            {
                HitParameter hitParameter = new HitParameter(caster.Status.StatusAttributes, damageMultiplier, 1.0f, 0);
                target.OnHit(ref hitParameter);
            }
        }
    }
}
