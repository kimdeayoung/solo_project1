using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class ActionObject : WorldObject
{
    private BattleUnit owner;
    private BaseActionData actionData;

    private Action<BaseActionData> onActionEnd;

    public void Init()
    {
        onActionEnd = OnActionEnd;
    }

    public void ResetVariables(BattleUnit owner, BaseActionData actionData)
    {
        this.owner = owner;
        Status = owner.Status;
        transform.SetPositionAndRotation(owner.transform.position, owner.transform.rotation);

        this.actionData = actionData;
    }

    public void Run()
    {
        actionData.SearchActionTarget(this);

        actionData.IncreaseRefCount();
        actionData.Execute(this, onActionEnd).Forget();
    }

    private void OnActionEnd(BaseActionData actionData)
    {
        BaseActionDataSO afterActionSO = actionData.AfterActionSO;
        if (afterActionSO != null)
        {
            BaseActionData afterAction = ActionDataPool.GetActionData(owner, afterActionSO);
            if (!TryExecuteAction(afterAction))
            {
                afterAction.DecreaseRefCount();
            }
        }

        Entitys entity = GameManager.Instance.SceneInstance<Battle>().Entity;
        entity.ReleaseActionObject(this);
    }

    public override bool TryExecuteAction(BaseActionData actionData)
    {
        if (actionData.IsSelfExecuted)
        {
            owner.TryExecuteAction(actionData);
            return true;
        }

        if (actionData.IsExecuteAble() && actionData.SearchActionTarget(this))
        {
            Entitys entity = GameManager.Instance.SceneInstance<Battle>().Entity;
            ActionObject actionObject = entity.GetActionObect(owner, actionData);
            actionObject.Run();
            return true;
        }
        return false;
    }

    public void Release()
    {
        actionData.DecreaseRefCount();
    }
}
