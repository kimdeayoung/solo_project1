using UnityEngine;

public abstract class ActionParameterSO : ScriptableObject
{
    [SerializeField] private float _runDelay;
    public float RunDelay => _runDelay;

    [SerializeField] private float _duration;
    public float Duration => _duration;

    [SerializeField] private string _animationName;
    public string AnimationName => _animationName;
}
