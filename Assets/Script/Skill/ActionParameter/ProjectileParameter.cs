using System.Collections.Generic;
using UnityEngine;

public class ProjectileParameter : ActionParameter
{
    public override ActionParameterType ActionParameterType => ActionParameterType.Projectile;

    protected override void RunAction_Impl(WorldObject caster, List<WorldObject> targets)
    {
    }
}
