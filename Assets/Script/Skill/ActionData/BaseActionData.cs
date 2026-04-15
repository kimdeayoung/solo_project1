using Cysharp.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

public readonly struct ActionResourceData
{
    public readonly float coolTime;
    public readonly ActionResourceType resourceType;
    public readonly int actionResource;

    public ActionResourceData(float coolTime, ActionResourceType resourceType, int actionResource)
    {
        this.coolTime = coolTime;
        this.resourceType = resourceType;
        this.actionResource = actionResource;
    }

    public ActionResourceData(BaseActionDataSO data) : this(data.CoolTime, data.ActionResourceType, data.ActionResource) { }
}

public abstract class BaseActionData
{
    public abstract ActionDataType ActionDataType { get; }

    protected BattleUnit owner;
    private ActionResourceData _resourceData;

    private float _coolTime;
    private bool _isUpdateAble;

    private float _runDelay;

    protected CancellationTokenSource cancelToken;

    public virtual void ResetVariables(BattleUnit owner, BaseActionDataSO data)
    {
        this.owner = owner;
        _resourceData = new ActionResourceData(data);

        _coolTime = _resourceData.coolTime;
        _isUpdateAble = true;

        _runDelay = data.RunDelay;

        if (cancelToken == null)
        {
            cancelToken = new CancellationTokenSource();
        }
    }

    public bool IsExecuteAble()
    {
        if (!owner.HasEnoughActionResource(_resourceData))
        {
            return false;
        }

        return _coolTime <= 0.0f;
    }

    public void OnUpdate(float deltaTime, float hasteValue)
    {
        if (!_isUpdateAble)
        {
            return;
        }

        float speedMultiplier = 1.0f + hasteValue;
        _coolTime -= deltaTime * speedMultiplier;
    }

    public async void Execute()
    {
        Debug.Assert(IsExecuteAble());

        _isUpdateAble = false;
        Debug.Assert(owner.HasEnoughActionResource(_resourceData));
        owner.UseActionResource(_resourceData);

        bool isCanceled = await UniTask.WaitForSeconds(_runDelay, cancellationToken: cancelToken.Token).SuppressCancellationThrow();

        float actionDuration = 0.0f;
        if (!isCanceled)
        {
            actionDuration = RunActions();
        }

        isCanceled = await UniTask.WaitForSeconds(actionDuration, cancellationToken: cancelToken.Token).SuppressCancellationThrow();
        if (!isCanceled)
        {
            _coolTime = _resourceData.coolTime;
            _isUpdateAble = true;
        }
    }

    protected abstract float RunActions();

    public void Stop()
    {
        _isUpdateAble = false;

        cancelToken.Cancel();
        cancelToken.Dispose();
        cancelToken = null;
    }

    public virtual void Release()
    {
        _isUpdateAble = false;
    }
}

public static class ActionDataPool
{
    private static List<BaseActionData>[] actionDatas;

    public static void Init()
    {
        int loopCount = (int)ActionDataType.Length;
        actionDatas = new List<BaseActionData>[loopCount];

        for (int i = 0; i < loopCount; i++)
        {
            actionDatas[i] = new List<BaseActionData>(32);
        }
    }

    public static BaseActionData GetActionData(BattleUnit owner, BaseActionDataSO data)
    {
        List<BaseActionData> list = actionDatas[(int)data.ActionType];

        BaseActionData actionData = null;
        if (list.Count > 0)
        {
            int index = list.Count - 1;
            actionData = list[index];
            list.RemoveAt(index);
        }
        else
        {
            actionData = CreateActionData(data.ActionType);
        }
        actionData.ResetVariables(owner, data);
        Debug.Assert(actionData != null);
        return actionData;
    }

    private static BaseActionData CreateActionData(ActionDataType type)
    {
        switch (type)
        {
            case ActionDataType.Default:
                return new DefaultActionData();

            case ActionDataType.Random:
                return new RandomActionData();
        }

        return null;
    }

    public static void Release(BaseActionData actionData)
    {
        ActionDataType type = actionData.ActionDataType;
        List<BaseActionData> list = actionDatas[(int)type];
        list.Add(actionData);
    }

    public static void Clear()
    {
        int loopCount = (int)ActionDataType.Length;
        for (int i = 0; i < loopCount; i++)
        {
            actionDatas[i].Clear();
        }
    }
}