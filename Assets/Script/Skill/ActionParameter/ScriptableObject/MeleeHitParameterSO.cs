using UnityEngine;

[CreateAssetMenu(fileName = "MeleeHitParameter", menuName = "Scriptable Objects/ActionParameter/MeleeHit")]
public class MeleeHitParameterSO : ActionParameterSO
{
    [SerializeField] private float damageMultiplier;
    public float DamageMultiplier => damageMultiplier;

    [SerializeField] private float hitThresholdAngle;
    public float HitThresholdAngle => hitThresholdAngle;

    public override ActionParameterType ActionParameterType => ActionParameterType.MeleeHit;
}
