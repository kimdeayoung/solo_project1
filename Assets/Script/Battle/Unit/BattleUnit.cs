using UnityEngine;

public abstract class BattleUnit : WorldObject
{
    public abstract BattleUnitType Type { get; }
    public ActionResourceType ResourceType { get; protected set; }
    public int ActionResource { get; protected set; }

    public string AssetName { get; private set; }

    public IBehaviourController BehaviourController { get; private set; }

    private Vector3 moveDirection;
    private float moveIntensity;

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

    public void SetMoveDirection(Vector3 direction, float intensity)
    {
        moveDirection = direction;
        moveIntensity = Mathf.Clamp01(intensity);
    }

    public void TranslateWithRotation(float fixedDeltaTime)
    {
        if (moveIntensity <= float.Epsilon)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        Quaternion newRotation = Quaternion.RotateTowards(rigidBody.rotation, targetRotation, Status.StatusAttributes.RotateSpeed * fixedDeltaTime);

        rigidBody.MoveRotation(newRotation);

        Vector3 forward = rigidBody.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 move = forward * Status.StatusAttributes.MoveSpeed * moveIntensity * fixedDeltaTime;
        rigidBody.MovePosition(rigidBody.position + move);
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
