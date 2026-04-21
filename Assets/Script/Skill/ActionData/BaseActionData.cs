using Cysharp.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine.Pool;
using System;

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
    private ActionResourceData resourceData;

    private SearchMethod searchMethod;
    private List<WorldObject> searchResult;

    private bool searchIgnoreCaster;
    private HashSet<int> searchIngoreTarget;

    private float coolTime;
    public float CooltimeRatio => coolTime / resourceData.coolTime;

    private bool isUpdateAble;

    private float runDelay;

    public bool IsSelfExecuted { get; private set; }
    public string IconName { get; private set; }
    public BaseActionDataSO AfterActionSO { get; private set; }

    protected CancellationTokenSource cancelToken;

    private int refCount;

    public virtual void ResetVariables(BattleUnit owner, BaseActionDataSO data)
    {
        Entitys entitys = GameManager.Instance.SceneInstance<Battle>().Entity;
        Debug.Assert(entitys != null);

        this.owner = owner;
        resourceData = new ActionResourceData(data);

        searchMethod = entitys.GetSearchMethod(data.SearchMethodType, data.SearchMethod);
        searchIgnoreCaster = data.SearchIgnoreCaster;

        IsSelfExecuted = data.IsSelfExecuted;

        coolTime = resourceData.coolTime;
        isUpdateAble = true;

        searchIngoreTarget = new HashSet<int>(8);

        runDelay = data.RunDelay;

        IconName = data.IconName;
        AfterActionSO = data.AfterActionSO;

        searchResult = new List<WorldObject>();
        if (cancelToken == null)
        {
            cancelToken = new CancellationTokenSource();
        }
    }

    public bool IsExecuteAble()
    {
        if (!isUpdateAble)
        {
            return false;
        }

        if (!owner.HasEnoughActionResource(resourceData))
        {
            return false;
        }

        return coolTime <= 0.0f;
    }

    public void OnUpdate(float deltaTime, float hasteValue)
    {
        if (!isUpdateAble)
        {
            return;
        }

        float speedMultiplier = 1.0f + hasteValue;
        coolTime -= deltaTime * speedMultiplier;
    }

    public bool SearchActionTarget()
    {
        if (searchIgnoreCaster)
        {
            searchIngoreTarget.Add(owner.GetInstanceID());
            searchMethod.Run(owner, searchIngoreTarget, searchResult);
        }
        else
        {
            searchMethod.Run(owner, null, searchResult);
        }
        return searchResult.Count > 0;
    }

    public bool SearchActionTarget(WorldObject caster)
    {
        Debug.Assert(caster != null);
        Debug.Assert(searchIngoreTarget != null);

        if (searchIgnoreCaster)
        {
            searchIngoreTarget.Add(caster.GetInstanceID());
        }

        searchMethod.Run(caster, searchIngoreTarget, searchResult);
        return searchResult.Count > 0;
    }

    public bool SearchActionTarget(SearchMethod customeSearchMethod, WorldObject caster)
    {
        Debug.Assert(caster != null);
        Debug.Assert(searchIngoreTarget != null);

        if (searchIgnoreCaster)
        {
            searchIngoreTarget.Add(caster.GetInstanceID());
        }

        customeSearchMethod.Run(caster, searchIngoreTarget, searchResult);
        return searchResult.Count > 0;
    }

    public async UniTask Execute(WorldObject caster, Action<BaseActionData> onComplete = null)
    {
        Debug.Assert(IsExecuteAble());
        Debug.Assert(searchResult.Count > 0);

        isUpdateAble = false;
        Debug.Assert(owner.HasEnoughActionResource(resourceData));
        owner.UseActionResource(resourceData);

        bool isCanceled = await UniTask.WaitForSeconds(runDelay, cancellationToken: cancelToken.Token).SuppressCancellationThrow();

        float actionDuration = 0.0f;
        if (!isCanceled)
        {
            actionDuration = RunActions(caster, searchResult);
        }

        if (actionDuration > 0.0f)
        {
            await UniTask.WaitForSeconds(actionDuration, cancellationToken: cancelToken.Token).SuppressCancellationThrow();
        }

        coolTime = resourceData.coolTime;
        isUpdateAble = true;

        searchResult.Clear();
        searchIngoreTarget.Clear();

        onComplete?.Invoke(this);
    }

    protected abstract float RunActions(WorldObject caster, List<WorldObject> searchResult);

    public void Stop()
    {
        isUpdateAble = false;

        cancelToken.Cancel();
        cancelToken.Dispose();
        cancelToken = null;
    }

    public void IncreaseRefCount()
    {
        ++refCount;
    }

    public void DecreaseRefCount()
    {
        if (--refCount == 0)
        {
            ActionDataPool.Release(this);
        }
    }

    protected virtual void Release()
    {
        isUpdateAble = false;

        Entitys entitys = GameManager.Instance.SceneInstance<Battle>().Entity;
        Debug.Assert(entitys != null);

        entitys.ReleaseSearchMethod(searchMethod);

        searchResult.Clear();
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