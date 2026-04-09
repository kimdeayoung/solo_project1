using UnityEngine;

public abstract class StatusInfluence
{
    public abstract CrowdControlType InfluenceType { get; }
    public float Duration { get; protected set; }

    public abstract void OnStart(BattleUnit unit);
    public abstract void OnEnd(BattleUnit unit);

    public abstract void AddInfluence();
    public abstract void RemoveInfluence();

    public abstract void OnUpdate(float deltaTime);
}
