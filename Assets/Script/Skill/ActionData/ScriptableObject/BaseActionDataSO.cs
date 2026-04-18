using UnityEngine;
using UnityEngine.AddressableAssets;


public abstract class BaseActionDataSO : ScriptableObject
{
    public abstract ActionDataType ActionType { get; }

    [SerializeField] private SearchMethodType searchMethodType;
    public SearchMethodType SearchMethodType => searchMethodType;

    [SerializeField] private SearchMethodProperty searchMethod;
    public SearchMethodProperty SearchMethod => searchMethod;

    [SerializeField] private bool searchIgnoreCaster;
    public bool SearchIgnoreCaster => searchIgnoreCaster;

    [SerializeField] private float coolTime;
    public float CoolTime => coolTime;

    [SerializeField] private ActionResourceType actionResourceType;
    public ActionResourceType ActionResourceType => actionResourceType;
    [SerializeField] private int actionResource;
    public int ActionResource => actionResource;

    [SerializeField] private ActionParameterSO[] actionParameters;
    public ActionParameterSO[] ActionParameters => actionParameters;

    [SerializeField] private float runDelay;
    public float RunDelay => runDelay;

    [SerializeField] private ObjectReference<Sprite> icon;
    public string IconName => icon.AssetName;

    [SerializeField] private BaseActionDataSO _afterActionSO;
    public BaseActionDataSO AfterActionSO => _afterActionSO;
}
