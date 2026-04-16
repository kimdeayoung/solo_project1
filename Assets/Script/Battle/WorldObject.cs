using UnityEngine;

public abstract class WorldObject : MonoBehaviour
{
    public UnitStatus Status { get; protected set; }

    public bool IsAlive()
    {
        return Status.IsAlive();
    }
}
