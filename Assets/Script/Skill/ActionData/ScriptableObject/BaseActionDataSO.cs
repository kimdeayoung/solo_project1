using UnityEngine;


public abstract class BaseActionDataSO : ScriptableObject
{
    public abstract ActionDataType ActionType { get; }

    [SerializeField] private float _coolTime;
    public float CoolTime => _coolTime;

    [SerializeField] private ActionResourceType _actionResourceType;
    public ActionResourceType ActionResourceType => _actionResourceType;
    [SerializeField] private int _actionResource;
    public int ActionResource => _actionResource;

    [SerializeField] private ActionParameter[] _actionParameters;
    public ActionParameter[] ActionParameters => _actionParameters;

    [SerializeField] private float _runDelay;
    public float RunDelay => _runDelay;

    [SerializeField] private BaseActionDataSO _afterActionSO;
    public BaseActionDataSO AfterActionSO => _afterActionSO;
}
