using System.Collections.Generic;
using UnityEngine;

public interface IBehaviourController
{
    UnitState CurrentUnitState { get; }

    IBehaviourController AddBehaviourState<T>(BattleUnit owner) where T : BehaviourState, new();
    void SetBehaviourState(UnitState state);
    void OnUpdate(float deltaTime);
    void OnFixedUpdate(float fixedDeltaTime);
}

public class BehaviourController : IBehaviourController
{
    private BehaviourState CurrentState;
    public UnitState CurrentUnitState => CurrentState.UnitState;

    private readonly Dictionary<UnitState, BehaviourState> _behaviourStates = new Dictionary<UnitState, BehaviourState>(8);

    public IBehaviourController AddBehaviourState<T>(BattleUnit owner) where T : BehaviourState, new()
    {
        BehaviourState behaviourState = new T();
        behaviourState.Init(owner);

        _behaviourStates.Add(behaviourState.UnitState, behaviourState);

        return this;
    }

    public void SetBehaviourState(UnitState state)
    {
        if (CurrentState != null)
        {
            if (CurrentState.UnitState == state)
            {
                return;
            }

            CurrentState.OnEnd();
        }

        _behaviourStates.TryGetValue(state, out BehaviourState newState);
        Debug.Assert(newState != null);
        Debug.Assert(newState.UnitState.Equals(state));

        CurrentState = newState;
        CurrentState.OnStart();
    }

    public void OnUpdate(float deltaTime)
    {
        Debug.Assert(CurrentState != null);
        CurrentState.OnUpdate(deltaTime);
    }

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        CurrentState.OnFixedUpdate(fixedDeltaTime);
    }
}