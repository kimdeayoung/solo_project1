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

    public abstract void OnFixedUpdate(float fixedDeltaTime);

    public bool IsAlive()
    {
        return Status.IsAlive();
    }

    public virtual void OnHit(in HitParameter hitParameter)
    {
    }

    public void TryApplyKnockback(WorldObject target)
    {
        UnitStatusGlobalVariables values = GameManager.Instance.GlobalVariables.UnitStatusGlobalVariables;

        int value = Status.StatusAttributes.Weight - target.Status.StatusAttributes.Weight;
        if (value <= values.ApplyKnockbackValue)
        {
            return;
        }

        float duration = Random.Range(values.ApplyKnockbackDuration.x, values.ApplyKnockbackDuration.y);
        target.Status.ApplyStatusInfluence(this, new AddStatusInfluenceData(AddStatusInfluenceType.Unique, StatusInfluenceType.Knockback, duration, value));
    }

    public virtual bool TryExecuteAction(BaseActionData actionData)
    {
        return false;
    }
}
