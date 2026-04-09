using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public interface IBehaviourController
{
    UnitState CurrentUnitState { get; }

    IBehaviourController AddBehaviourState<T>(BattleUnit owner) where T : BehaviourState, new();
    void SetBehaviourState(UnitState state);
    void OnUpdate(float deltaTime);
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
        Assert.IsNotNull(CurrentState);
        CurrentState.OnEnd();

        _behaviourStates.TryGetValue(state, out BehaviourState newState);
        Assert.IsNotNull(newState);
        Assert.IsFalse(newState.UnitState.Equals(state));

        CurrentState = newState;
        CurrentState.OnStart();
    }

    public void OnUpdate(float deltaTime)
    {
        Assert.IsNotNull(CurrentState);
        CurrentState.OnUpdate(deltaTime);
    }
}