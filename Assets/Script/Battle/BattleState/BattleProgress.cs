using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleProgress : BattleState
{
    public BattleProgress(BattleController battleController) : base(battleController)
    {
    }

    public override void StateEnter()
    {
    }

    public override void StateExit()
    {
    }

    public override void StateUpdate(float deltaTime)
    {
        IReadOnlyList<BattleUnit> battleUnits = controller.BattleUnits;
        int loopCount = battleUnits.Count;
        for (int i = 0; i < loopCount; i++)
        {
            battleUnits[i].OnUpdate(deltaTime);
        }
    }

    public override void StateFixedUpdate(float fixedDeltaTime)
    {
        IReadOnlyList<BattleUnit> battleUnits = controller.BattleUnits;
        int loopCount = battleUnits.Count;
        for (int i = 0; i < loopCount; i++)
        {
            battleUnits[i].OnFixedUpdate(fixedDeltaTime);
        }
    }
}
