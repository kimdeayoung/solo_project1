using UnityEngine;

public abstract class BattleUnit : MonoBehaviour
{
    public abstract BattleUnitType Type { get; }
    public ActionResourceType ResourceType { get; protected set; }
    public int ActionResource { get; protected set; }

    public string AssetName { get; private set; }

    public UnitStatus Status { get; protected set; }
    public IBehaviourController BehaviourController { get; private set; }

    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody rigidBody;

    public virtual void Init(string assetName)
    {
        BehaviourController = new BehaviourController();

        AssetName = assetName;
    }

    public virtual void OnStart()
    {
    }

    public abstract void OnUpdate(float deltaTime);
    public abstract void OnFixedUpdate(float fixedDeltaTime);

    public bool IsAlive()
    {
        return Status.IsAlive();
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

    public virtual void SetMoveDirection(Vector3 direction, float intensity)
    {
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
