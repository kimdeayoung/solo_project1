using NUnit.Framework;
using UnityEngine;

public abstract class StatusInfluence
{
    protected BattleUnit unit;
    public abstract StatusInfluenceType InfluenceType { get; }
    public float Duration { get; protected set; }

    public virtual void OnStart(BattleUnit unit)
    {
        this.unit = unit;
    }

    protected abstract void OnEnd();

    public virtual void AddInfluence(AddStatusInfluenceData data)
    {
        switch (data.addStatusInfluenceType)
        {
            case AddStatusInfluenceType.Independent:
                Assert.True(false);
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

    public abstract bool OnUpdate(float deltaTime);
}
