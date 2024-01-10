using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInit : BattleState
{
    private BattleUIController battleUIController;

    public BattleInit(BattleController battleController) : base(battleController)
    {
        battleUIController = battleController.BattleUIController;
    }

    public override void StateEnter()
    {

    }

    public override void StateUpdate()
    {
        if (battleUIController.BattleUILoadState == LoadState.Complete)
        {
            controller.SetBattleState(BattleStateType.Progress);
        }
    }

    public override void StateExit()
    {

    }
}
