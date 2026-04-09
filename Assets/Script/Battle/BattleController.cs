using System.Collections.Generic;
using UnityEngine;

public struct BattleStartData
{
    public TRStage trStage;
}

public class BattleController
{
    public BattleStartData BattleStartData { get; private set; }
    protected BattleState _currentBattleState;

    public BattleUIController BattleUIController { get; protected set; }

    public readonly List<BattleUnit> _battleUnits = new List<BattleUnit>(256);
    public IReadOnlyList<BattleUnit> BattleUnits => _battleUnits;
    

    public BattleController(BattleStartData battleStartData)
    {
        BattleUIController = new BattleUIController(this);
        BattleStartData = battleStartData;
    }

    public void CrateBattleObjects()
    {
        SetBattleState(BattleStateType.BattleInit);
    }

    public void SetBattleState(BattleStateType type)
    {
        switch (type)
        {
            case BattleStateType.BattleInit:
                _currentBattleState = new BattleInit(this);
                break;
            case BattleStateType.Progress:
                _currentBattleState = new BattleProgress(this);
                break;
            case BattleStateType.Victory:
                _currentBattleState = new BattleVictory(this);
                break;
            case BattleStateType.Defeat:
                _currentBattleState = new BattleDefeat(this);
                break;
            default:
                Debug.Assert(false);
                break;
        }
        _currentBattleState.StateEnter();
    }

    public void AddBattleUnit(BattleUnit battleUnit)
    {
        _battleUnits.Add(battleUnit);
    }

    public void Update()
    {
        Debug.Assert(_currentBattleState != null);

        float deltaTime = Time.deltaTime * TimeManager.Instance.GameSpeed;
        _currentBattleState.StateUpdate(deltaTime);
    }

    public void FixedUpdate()
    {
        _currentBattleState.StateFixedUpdate(Time.fixedDeltaTime);
    }

    public void ClearData()
    {
        BattleUIController.OnDestroy();
        BattleUIController = null;
    }
}
