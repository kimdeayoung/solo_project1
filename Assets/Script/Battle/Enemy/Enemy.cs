using Cysharp.Threading.Tasks;
using EnemyState;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BattleUnit
{
    [SerializeField] private EnemyStatusStat _stat;

    public WorldObject Target { get; private set; }
    private List<BaseActionData> _actionDatas;
    public IReadOnlyList<BaseActionData> ActionDatas => _actionDatas;

    public override void Init(string assetName)
    {
        base.Init(assetName);

        BehaviourController.AddBehaviourState<ChaseState>(this)
                           .AddBehaviourState<ActionState>(this);
        BehaviourController.SetBehaviourState(UnitState.Chase);

        Status = new EnemyStatus(this, _stat);
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
    }

    public override BattleUnitType Type => BattleUnitType.Enemy;

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

        for (int i = 0; i < loopCount; i++)
        {
            BaseActionData actionData = _actionDatas[i];
            if (actionData.IsExecuteAble() && actionData.SearchActionTarget())
            {
                BehaviourController.SetBehaviourState(UnitState.Action);
                actionData.Execute().Forget();
                break;
            }
        }
    }
}
