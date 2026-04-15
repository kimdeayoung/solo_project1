using UnityEngine;

public abstract class ActionParameterSO : ScriptableObject
{
    [SerializeField] private int parameterUniqueID;
    public int ParameterUniqueID => parameterUniqueID;

    [SerializeField] private float runDelay;
    public float RunDelay => runDelay;

    [SerializeField] private float duration;
    public float ActionDuration => duration;

    [SerializeField] private string animationName;
    public string AnimationName => animationName;

    public abstract ActionParameterType ActionParameterType { get; }
}
