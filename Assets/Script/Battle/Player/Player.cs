using Cysharp.Threading.Tasks;
using PlayerState;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BattleUnit
{
    [SerializeField] private PlayerStatusStat _stat;
    [SerializeField] private float _hitApplyDotValue;

    public override BattleUnitType Type => BattleUnitType.Player;

    private Vector3 moveDirection;
    private float moveIntensity;

    private Action<BaseActionData> onActionEnd;

    public override void Init(string assetName)
    {
        base.Init(assetName);

        BehaviourController.AddBehaviourState<IdleState>(this)
                           .AddBehaviourState<StunState>(this)
                           .AddBehaviourState<DeadState>(this);
        BehaviourController.SetBehaviourState(UnitState.Idle);

        Status = new PlayerStatus(this, _stat);
        rigidBody.mass = Status.StatusAttributes.Weight;

        onActionEnd = OnActionEnd;
    }

    public override void OnStart()
    {
        base.OnStart();

        {
            BaseActionDataSO[] actionSOs = _stat.ActionDatas;
            int actionDataCount = actionSOs.Length;
            actionDatas = new List<BaseActionData>(actionDataCount);
            for (int i = 0; i < actionDataCount; i++)
            {
                BaseActionData actionData = ActionDataPool.GetActionData(this, actionSOs[i]);
                actionData.IncreaseRefCount();
                actionDatas.Add(actionData);
            }
        }

        {
            BaseActionDataSO[] collisionActionSOs = _stat.CollisionActions;
            int collisionActionsCount = collisionActionSOs.Length;
            collisionActions = new List<BaseActionData>(collisionActionsCount);
            for (int i = 0; i < collisionActionsCount; i++)
            {
                BaseActionData actionData = ActionDataPool.GetActionData(this, collisionActionSOs[i]);
                actionData.IncreaseRefCount();
                collisionActions.Add(actionData);
            }
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        BehaviourController.OnUpdate(deltaTime);
    }

    public override void OnFixedUpdate(float fixedDeltaTime)
    {
        base.OnFixedUpdate(fixedDeltaTime);
        BehaviourController.OnFixedUpdate(fixedDeltaTime);
    }

    protected override bool IsHitAble(WorldObject target, out int weightOffset)
    {
        weightOffset = 0;

        float dot = Vector3.Dot(transform.forward, Vector3.Normalize(target.transform.position - transform.position));
        if (dot < _hitApplyDotValue)
        {
            return false;
        }

        UnitStatusGlobalVariables values = GameManager.Instance.GlobalVariables.UnitStatusGlobalVariables;
        weightOffset = Status.StatusAttributes.Weight - target.Status.StatusAttributes.Weight;
        if (weightOffset <= values.ApplyKnockbackValue)
        {
            return false;
        }
        return true;
    }

    public override void OnHit(ref HitParameter hitParameter)
    {
        base.OnHit(ref hitParameter);

        if (IsAlive())
        {
        }
        else
        {
            BehaviourController.SetBehaviourState(UnitState.Dead);
        }
    }

    public override void UpdateActionDatas(float deltaTime)
    {
        int loopCount = actionDatas.Count;
        for (int i = 0; i < loopCount; i++)
        {
            actionDatas[i].OnUpdate(deltaTime, Status.GetHaste());
        }
    }

    public void SetMoveDirection(Vector3 direction, float intensity)
    {
        moveDirection = direction;
        moveIntensity = Mathf.Clamp01(intensity);
    }

    public void MoveWithRotation(float fixedDeltaTime)
    {
        if (moveIntensity <= float.Epsilon)
        {
            rigidBody.linearVelocity = Vector3.zero;
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        Quaternion newRotation = Quaternion.RotateTowards(rigidBody.rotation, targetRotation, Status.StatusAttributes.RotateSpeed * fixedDeltaTime);

        rigidBody.MoveRotation(newRotation);

        Vector3 moveStep = transform.forward * Status.StatusAttributes.MoveSpeed * moveIntensity;
        rigidBody.linearVelocity = moveStep;
    }

    public override bool TryExecuteAction(BaseActionData actionData)
    {
        if (actionData.IsExecuteAble())
        {
            if (actionData.IsSelfExecuted)
            {
                if (actionData.SearchActionTarget())
                {
                    actionData.IncreaseRefCount();
                    actionData.Execute(this, onActionEnd).Forget();
                    return true;
                }
            }
            else
            {
                Entitys entity = GameManager.Instance.SceneInstance<Battle>().Entity;
                ActionObject actionObject = entity.GetActionObect(this, actionData);
                actionObject.Run();
                return true;
            }
        }
        return false;
    }

    private void OnActionEnd(BaseActionData actionData)
    {
        BaseActionDataSO afterActionSO = actionData.AfterActionSO;
        if (afterActionSO != null)
        {
            BaseActionData afterAction = ActionDataPool.GetActionData(this, afterActionSO);
            if (!TryExecuteAction(afterAction))
            {
                afterAction.DecreaseRefCount();
            }
        }
    }
}
