using PlayerState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BattleUnit
{
    [SerializeField] private PlayerStatusStat _stat;

    public override BattleUnitType Type =>  BattleUnitType.Player;

    private List<BaseActionData> _actionDatas;
    public IReadOnlyList<BaseActionData> ActionDatas => _actionDatas;

    public override void Init(string assetName)
    {
        base.Init(assetName);

        BehaviourController.AddBehaviourState<IdleState>(this)
                           .AddBehaviourState<StunState>(this);
        BehaviourController.SetBehaviourState(UnitState.Idle);

        Status = new PlayerStatus(this, _stat);
    }

    public override void OnStart()
    {
        base.OnStart();

        {
            BaseActionDataSO[] actionDatas = _stat.ActionDatas;
            int actionDataCount = actionDatas.Length;
            _actionDatas = new List<BaseActionData>(actionDataCount);
            for (int i = 0; i < actionDataCount; i++)
            {
                BaseActionData actionData = ActionDataPool.GetActionData(this, actionDatas[i]);
                _actionDatas.Add(actionData);
            }
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        BehaviourController.OnUpdate(deltaTime);
    }

    public override void OnFixedUpdate(float fixedDeltaTime)
    {
        BehaviourController.OnFixedUpdate(fixedDeltaTime);
    }

    public override void UpdateActionDatas(float deltaTime)
    {
        int loopCount = _actionDatas.Count;
        for (int i = 0; i < loopCount; i++)
        {
            _actionDatas[i].OnUpdate(deltaTime, Status.GetHaste());
        }
    }
}
