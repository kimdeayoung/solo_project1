using UnityEngine;

public readonly struct HitParameter
{
    public readonly float atk;
    public readonly float defT0;
    public readonly float defT1;

    public readonly float damageMultiplier;

    public readonly float accelerationRatio;
    public readonly int weightOffset;

    public HitParameter(StatusAttributes statusAttributes, float damageMultiplier, float accelerationRatio, int weightOffset)
    {
        atk = statusAttributes.Atk;
        defT0 = statusAttributes.DefT0;
        defT1 = statusAttributes.DefT1;

        this.damageMultiplier = damageMultiplier;
        this.accelerationRatio = accelerationRatio;
        this.weightOffset = weightOffset;
    }

    public HitParameter(float atk, float defT0, float defT1, float damageMultiplier, float accelerationRatio, int weightOffset)
    {
        this.atk = atk;
        this.defT0 = defT0;
        this.defT1 = defT1;

        this.damageMultiplier = damageMultiplier;
        this.accelerationRatio = accelerationRatio;
        this.weightOffset = weightOffset;
    }

    public int GetDamage()
    {
        float value = (atk - defT0) * damageMultiplier;
        value -= value * defT1;
        value *= Mathf.Clamp01(accelerationRatio);

        return Mathf.CeilToInt(value);
    }
}
