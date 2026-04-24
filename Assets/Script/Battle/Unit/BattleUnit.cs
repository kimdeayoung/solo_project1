using UnityEngine;

public abstract class BattleUnit : WorldObject
{
    public abstract BattleUnitType Type { get; }
    public ActionResourceType ResourceType { get; protected set; }
    public int ActionResource { get; protected set; }

    public string AssetName { get; private set; }

    public IBehaviourController BehaviourController { get; private set; }

    [SerializeField] protected Animator animator;

    public override void Init(string assetName)
    {
        base.Init(assetName);
        BehaviourController = new BehaviourController();

        AssetName = assetName;
    }

    public virtual bool HasEnoughActionResource(ActionResourceData data)
    {
        if (data.resourceType == ResourceType)
        {
            return ActionResource >= data.actionResource;
        }

        return false;
    }

    public virtual void UseActionResource(ActionResourceData data)
    {
        Debug.Assert(HasEnoughActionResource(data));

        ActionResource -= data.actionResource;
    }

    public virtual void UpdateActionDatas(float deltaTime)
    {
    }

    public void RunAnimation(string animationName)
    {
    }

    public virtual void Release()
    {

    }
}
