using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Assertions;

public class BattleController
{
    protected TRStage trStage;

    protected BattleUIController battleUIController;

    protected UserControllerData userControllerData;

    protected BattleState currentBattleState;

    public BattleUIController BattleUIController { get => battleUIController; }
    public UserControllerData UserControllerData { get => userControllerData; set => userControllerData = value; }

    public BattleController(TRStage trStage)
    {
        battleUIController = new BattleUIController(this, trStage);
        this.trStage = trStage;
    }

    public void CrateBattleObjects()
    {
        SetBattleState(BattleStateType.BattleInit);


        battleUIController.CreateBattleUI();
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
        userControllerData = null;

        battleUIController.OnDestroy();
        battleUIController = null;
    }
}
