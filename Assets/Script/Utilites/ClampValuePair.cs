using UnityEngine;

public readonly struct ClampValuePair<T1, T2>
{
    public readonly T1 OrigianlValue;
    public readonly T2 ClampValue;

    public ClampValuePair(T1 original, T2 clamp)
    {
        OrigianlValue = original;
        ClampValue = clamp;
    }
}
