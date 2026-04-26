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
            float dotValue = Vector3.Dot(caster.transform.forward, target.transform.forward);
            if (dotValue < hitThresholdAngle)
            {
                HitParameter hitParameter = new HitParameter(caster.Status.StatusAttributes, damageMultiplier);
                target.OnHit(ref hitParameter);
            }
        }
    }
}
