using UnityEngine;

public class ProjectileParameter : ActionParameter
{
    public override ActionParameterType ActionParameterType => ActionParameterType.Projectile;

    protected override void RunAction_Impl(BattleUnit target)
    {
    }
}
