using PlayerState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BattleUnit
{
    [SerializeField] private PlayerStatusStat _stat;

    public UserControllerData UserControllerData { get; private set; }

    public override BattleUnitType Type =>  BattleUnitType.Player;

    private List<BaseActionData> _actionDatas;

    public override void Init(string assetName)
    {
        base.Init(assetName);

        //UserControllerData = new UserControllerData(battleUIController.Joystick, battleUIController.ActionBtns);

        BehaviourController.AddBehaviourState<IdleState>(this)
                           .AddBehaviourState<StunState>(this);
        
        Status = new PlayerStatus(this, _stat);
    }

    public override void OnStart()
    {
        base.OnStart();

        BehaviourController.SetBehaviourState(UnitState.Idle);

        {
            BaseActionDataSO[] actionDatas = _stat.ActionDatas;
            int actionDataCount = actionDatas.Length;
            _actionDatas = new List<BaseActionData>(actionDataCount);
            for (int i = 0; i < actionDataCount; i++)
            {
                BaseActionDataSO actionDataSO = actionDatas[i];
                BaseActionData actionData = ActionDataPool.GetActionParameter(actionDataSO.ActionType);

                actionData.Init(this);
                actionData.ResetVariables(actionDataSO);
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
            _actionDatas[i].OnUpdate(deltaTime, Status.Haste);
        }
    }

    public void TranslateWithRotation(Vector3 targetDir, float fixedDeltaTime)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);
        Quaternion newRotation = Quaternion.RotateTowards(rigidBody.rotation, targetRotation, _stat.Rotate * fixedDeltaTime);

        rigidBody.MoveRotation(newRotation);

        Vector3 forward = rigidBody.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 move = forward * _stat.MoveSpeed * fixedDeltaTime;
        rigidBody.MovePosition(rigidBody.position + move);
    }
}
