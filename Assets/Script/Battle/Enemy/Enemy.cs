using Cysharp.Threading.Tasks;
using EnemyState;
using PlayerState;
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
    [SerializeField] private EnemyStatusStat _stat;
    [SerializeField] private NavMeshAgent _agent;

    public WorldObject Target { get; private set; }
    private List<BaseActionData> _actionDatas;
    public IReadOnlyList<BaseActionData> ActionDatas => _actionDatas;

    private Action<BaseActionData> onActionEnd;

    public override void Init(string assetName)
    {
        base.Init(assetName);

        BehaviourController.AddBehaviourState<EnemyState.IdleState>(this)
                           .AddBehaviourState<ActionState>(this)
                           .AddBehaviourState<StunState>(this);
        BehaviourController.SetBehaviourState(UnitState.Idle);

        Status = new EnemyStatus(this, _stat);
        rigidBody.mass = Status.StatusAttributes.Weight;
        onActionEnd = OnActionEnd;
    }

    public override void OnStart()
    {
        base.OnStart();

        Target = GameManager.Instance.SceneInstance<Battle>().Entity.Player;
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

        ResetAgentProperty(new NavMeshAgentProperty(Status.StatusAttributes.MoveSpeed, 
                                                    Status.StatusAttributes.RotateSpeed));

        _agent.SetDestination(Target.transform.position);
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
        _agent.enabled = isEnable;
    }

    public void ChaseTarget()
    {
        _agent.SetDestination(Target.transform.position);
    }

    public override void UpdateActionDatas(float deltaTime)
    {
        int loopCount = _actionDatas.Count;
        for (int i = 0; i < loopCount; i++)
        {
            _actionDatas[i].OnUpdate(deltaTime, Status.GetHaste());
        }

        for (int i = 0; i < loopCount; i++)
        {
            if (TryExecuteAction(_actionDatas[i]))
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
        _agent.speed = property.speed;
        _agent.angularSpeed = property.rotateSpeed;
    }

    public override void OnHit(in HitParameter hitParameter)
    {
        base.OnHit(hitParameter);

        Debug.Log("On Hit Enemy");
    }
}
