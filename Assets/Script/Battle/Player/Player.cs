using PlayerState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BattleUnit
{
    [SerializeField] private PlayerStatusStat _stat;

    public override BattleUnitType Type =>  BattleUnitType.Player;

    private Vector3 moveDirection;
    private float moveIntensity;

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

    public override void SetMoveDirection(Vector3 direction, float intensity)
    {
        base.SetMoveDirection(direction, intensity);
        moveDirection = direction;
        moveIntensity = intensity;
    }

    public void TranslateWithRotation(float fixedDeltaTime)
    {
        if (moveIntensity <= float.Epsilon)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        Quaternion newRotation = Quaternion.RotateTowards(rigidBody.rotation, targetRotation, _stat.Rotate * fixedDeltaTime);

        rigidBody.MoveRotation(newRotation);

        Vector3 forward = rigidBody.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 move = forward * Status.StatusAttributes.MoveSpeed * moveIntensity * fixedDeltaTime;
        rigidBody.MovePosition(rigidBody.position + move);
    }
}
