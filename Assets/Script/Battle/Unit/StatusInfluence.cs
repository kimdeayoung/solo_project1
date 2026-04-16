using UnityEngine;

public abstract class StatusInfluence
{
    protected WorldObject owner;
    public abstract StatusInfluenceType InfluenceType { get; }
    public AddStatusInfluenceType AddStatusInfluenceType { get; private set; }
    public float Duration { get; protected set; }

    public virtual void OnStart(WorldObject unit, AddStatusInfluenceData data)
    {
        owner = unit;

        AddStatusInfluenceType = data.addStatusInfluenceType;
        Duration = data.duration;
    }

    protected abstract void OnEnd();

    public virtual void AddInfluence(AddStatusInfluenceData data)
    {
        switch (data.addStatusInfluenceType)
        {
            case AddStatusInfluenceType.Independent:
                Debug.Assert(false);
                break;
            case AddStatusInfluenceType.Stack:
                Duration += data.duration;
                break;
        }
    }

    public virtual void RemoveInfluence()
    {
        Duration = 0.0f;
    }

    public virtual bool OnUpdate(float deltaTime)
    {
        if (Duration > 0.0f)
        {
            Duration -= deltaTime;
        }

        if (Duration < 0)
        {
            OnEnd();
            return true;
        }
        return false;
    }
}
