using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleState
{
    protected BattleController controller;

    public BattleState(BattleController battleController)
    {
        controller = battleController;
    }

    public abstract void StateEnter();
    public abstract void StateUpdate();
    public abstract void StateExit();

}
