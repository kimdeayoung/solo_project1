using UnityEngine;

public abstract class WorldObject : MonoBehaviour
{
    public UnitStatus Status { get; protected set; }

    public bool IsAlive()
    {
        return Status.IsAlive();
    }

    public virtual void OnHit(in HitParameter hitParameter)
    {
    }

    public virtual bool TryExecuteAction(BaseActionData actionData)
    {
        return false;
    }
}
