using UnityEngine;

public abstract class BehaviourState
{
    protected BattleUnit ownerBase;
    public abstract UnitState UnitState { get; }

    public virtual void Init(BattleUnit owner)
    {
        this.ownerBase = owner;
    }

    public abstract void OnStart();
    public abstract void OnUpdate(float deltaTime);
    public abstract void OnFixedUpdate(float fixedDeltaTime);
    public abstract void OnEnd();
}
