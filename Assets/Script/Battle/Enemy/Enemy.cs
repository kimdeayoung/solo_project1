using Cysharp.Threading.Tasks;
using EnemyState;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public readonly struct NavMeshAgentProperty
{
    public readonly float speed;
    public readonly float rotateSpeed;

    public NavMeshAgentProperty(float speed, float rotateSpeed)
    {
        this.speed = speed;
        this.rotateSpeed = rotateSpeed;
    }
}

public class Enemy : BattleUnit
{
    [SerializeField] private EnemyStatusStat stat;
    [SerializeField] private float _hitApplyDotValue;

    [SerializeField] private NavMeshAgent agent;

    public WorldObject Target { get; private set; }

    private Action<BaseActionData> onActionEnd;

    public override void Init(string assetName)
    {
        base.Init(assetName);

        BehaviourController.AddBehaviourState<IdleState>(this)
                           .AddBehaviourState<ActionState>(this)
                           .AddBehaviourState<StunState>(this)
                           .AddBehaviourState<DeadState>(this);
        BehaviourController.SetBehaviourState(UnitState.Idle);

        Status = new EnemyStatus(this, stat);
        rigidBody.mass = Status.StatusAttributes.Weight;
        onActionEnd = OnActionEnd;

        agent.updatePosition = false;
        agent.updateRotation = true;
    }

    public override void OnStart()
    {
        base.OnStart();

        Target = GameManager.Instance.SceneInstance<Battle>().Entity.Player;
        {
            BaseActionDataSO[] actionDataSOs = stat.ActionDatas;
            int actionDataCount = actionDataSOs.Length;
            actionDatas = new List<BaseActionData>(actionDataCount);
            for (int i = 0; i < actionDataCount; i++)
            {
                BaseActionData actionData = ActionDataPool.GetActionData(this, actionDataSOs[i]);
                actionDatas.Add(actionData);
            }
        }

        ResetAgentProperty(new NavMeshAgentProperty(Status.StatusAttributes.MoveSpeed, 
                                                    Status.StatusAttributes.RotateSpeed));

        agent.SetDestination(Target.transform.position);
    }

    public override BattleUnitType Type => BattleUnitType.Enemy;

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

    public void SetEnableAgent(bool isEnable)
    {
        agent.enabled = isEnable;
    }

    public void ChaseTarget()
    {
        agent.SetDestination(Target.transform.position);
        agent.nextPosition = rigidBody.position;

        Vector3 moveStep = transform.forward * Status.StatusAttributes.MoveSpeed;
        rigidBody.linearVelocity = moveStep;
    }

    protected override bool IsHitAble(WorldObject target, out int weightOffset)
    {
        weightOffset = 0;
        if (target.gameObject.layer != LayerMaskCached.Player)
        {
            return false;
        }

        UnitStatusGlobalVariables values = GameManager.Instance.GlobalVariables.UnitStatusGlobalVariables;
        weightOffset = Status.StatusAttributes.Weight - target.Status.StatusAttributes.Weight;
        if (weightOffset > values.ApplyKnockbackValue)
        {
            return true;
        }

        float dot = Vector3.Dot(transform.forward, Vector3.Normalize(target.transform.position - transform.position));
        if (dot < _hitApplyDotValue)
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

        for (int i = 0; i < loopCount; i++)
        {
            if (TryExecuteAction(actionDatas[i]))
            {
                break;
            }
        }
    }

    public override bool TryExecuteAction(BaseActionData actionData)
    {
        if (actionData.IsExecuteAble())
        {
            if (actionData.IsSelfExecuted)
            {
                if (actionData.SearchActionTarget())
                {
                    BehaviourController.SetBehaviourState(UnitState.Action);
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
        if (afterActionSO == null)
        {
            BehaviourController.SetBehaviourState(UnitState.Idle);
        }
        else
        {
            BaseActionData afterAction = ActionDataPool.GetActionData(this, afterActionSO);
            if (!TryExecuteAction(afterAction))
            {
                afterAction.DecreaseRefCount();
            }
        }
    }

    public void ResetAgentProperty(NavMeshAgentProperty property)
    {
        agent.speed = property.speed;
        agent.angularSpeed = property.rotateSpeed;
    }
}
