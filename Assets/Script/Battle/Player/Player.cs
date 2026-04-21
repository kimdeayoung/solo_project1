using Cysharp.Threading.Tasks;
using PlayerState;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class Player : BattleUnit
{
    [SerializeField] private PlayerStatusStat _stat;

    public override BattleUnitType Type => BattleUnitType.Player;

    private Vector3 moveDirection;
    private float moveIntensity;

    private List<BaseActionData> _actionDatas;
    public IReadOnlyList<BaseActionData> ActionDatas => _actionDatas;

    private List<BaseActionData> _collisionActions;

    private Action<BaseActionData> onActionEnd;

    public override void Init(string assetName)
    {
        base.Init(assetName);

        BehaviourController.AddBehaviourState<IdleState>(this)
                           .AddBehaviourState<StunState>(this);
        BehaviourController.SetBehaviourState(UnitState.Idle);

        Status = new PlayerStatus(this, _stat);
        onActionEnd = OnActionEnd;
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
                actionData.IncreaseRefCount();
                _actionDatas.Add(actionData);
            }
        }

        {
            BaseActionDataSO[] collisionActions = _stat.CollisionActions;
            int collisionActionsCount = collisionActions.Length;
            _collisionActions = new List<BaseActionData>(collisionActionsCount);
            for (int i = 0; i < collisionActionsCount; i++)
            {
                BaseActionData actionData = ActionDataPool.GetActionData(this, collisionActions[i]);
                actionData.IncreaseRefCount();
                _collisionActions.Add(actionData);
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

    private void OnTriggerEnter(Collider collision)
    {
        WorldObject target = collision.gameObject.GetComponent<DetectableComponent>().WorldObject;
        HitParameter hitParameter = new HitParameter(Status.StatusAttributes, 1.0f);
        target.OnHit(hitParameter);

        int loopCount = _collisionActions.Count;
        for (int i = 0; i < loopCount; i++)
        {
            TryExecuteAction(_collisionActions[i]);
        }
    }

    public override void UpdateActionDatas(float deltaTime)
    {
        int loopCount = _actionDatas.Count;
        for (int i = 0; i < loopCount; i++)
        {
            _actionDatas[i].OnUpdate(deltaTime, Status.GetHaste());
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
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        Quaternion newRotation = Quaternion.RotateTowards(rigidBody.rotation, targetRotation, Status.StatusAttributes.RotateSpeed * fixedDeltaTime);

        rigidBody.MoveRotation(newRotation);

        Vector3 forward = newRotation * Vector3.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 moveStep = forward * Status.StatusAttributes.MoveSpeed * moveIntensity * fixedDeltaTime;
        rigidBody.MovePosition(rigidBody.position + moveStep);
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
