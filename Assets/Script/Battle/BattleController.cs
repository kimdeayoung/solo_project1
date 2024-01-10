using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public struct BattleStartData
{
    public TRStage trStage;
    public CharacterInfo playerCharacterInfo;
}

public class BattleController
{
    protected BattleStartData battleStartData;

    protected BattleState currentBattleState;

    protected BattleUIController battleUIController;

    protected BattleCharacter player;
    protected List<BattleUnit> enemys;

    public BattleUIController BattleUIController { get => battleUIController; }
    public BattleCharacter Player { get => player; set => player = value; }
    public BattleStartData BattleStartData { get => battleStartData; }

    public BattleController(BattleStartData battleStartData)
    {
        battleUIController = new BattleUIController(this);
        enemys = new List<BattleUnit>();
        this.battleStartData = battleStartData;
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
                currentBattleState = new BattleInit(this);
                break;
            case BattleStateType.Progress:
                currentBattleState = new BattleProgress(this);
                break;
            case BattleStateType.Victory:
                currentBattleState = new BattleVictory(this);
                break;
            case BattleStateType.Defeat:
                currentBattleState = new BattleDefeat(this);
                break;
            default:
                Assert.IsTrue(false);
                break;
        }
    }

    public void Update()
    {
        Assert.IsNotNull(currentBattleState);

        currentBattleState.StateUpdate();
    }

    public void ClearData()
    {
        battleUIController.OnDestroy();
        battleUIController = null;
    }
}
