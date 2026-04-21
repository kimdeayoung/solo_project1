using UnityEngine;

public readonly struct HitParameter
{
    public readonly StatusAttributes statusAttributes;

    public readonly float damageMultiplier;

    public HitParameter(StatusAttributes statusAttributes, float damageMultiplier)
    {
        this.statusAttributes = statusAttributes;
        this.damageMultiplier = damageMultiplier;
    }
}
