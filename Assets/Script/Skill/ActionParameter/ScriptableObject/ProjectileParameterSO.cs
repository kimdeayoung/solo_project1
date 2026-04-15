using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileParameter", menuName = "Scriptable Objects/ActionParameter/Projectile")]
public class ProjectileParameterSO : ActionParameterSO
{
    public override ActionParameterType ActionParameterType => ActionParameterType.Projectile;
}
