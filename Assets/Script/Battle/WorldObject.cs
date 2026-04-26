using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObject : MonoBehaviour
{
    [SerializeField] protected Rigidbody rigidBody;
    public Rigidbody Rigidbody => rigidBody;

    [SerializeField] protected Collider colliderComponent;

    public UnitStatus Status { get; protected set; }

    public virtual void Init(string assetName)
    {
    }

    public virtual void OnStart()
    {
    }

    public virtual void OnUpdate(float deltaTime)
    {
        Status.OnUpdate(deltaTime);
    }

    public virtual void OnFixedUpdate(float fixedDeltaTime)
    {
    }

    public bool IsAlive()
    {
        return Status.IsAlive();
    }

    public virtual void OnHit(ref HitParameter hitParameter)
    {
    }

    public virtual bool TryExecuteAction(BaseActionData actionData)
    {
        return false;
    }
}
