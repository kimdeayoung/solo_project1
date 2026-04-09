using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ActionParameter
{
    public abstract ActionParameterType ActionParameterType { get; }

    private float _runDelay;
    public float RunDelay => _runDelay;

    private float _duration;
    public float Duration => _duration;

    private string _animationName;
    public string AnimationName => _animationName;

    public virtual void ResetVariables(ActionParameterSO data)
    {
        _runDelay = data.RunDelay;

        _duration = data.Duration;

        _animationName = data.AnimationName;
    }

    public async void RunAction(BattleUnit target, CancellationToken token)
    {
        bool isCanceled = await UniTask.WaitForSeconds(_runDelay, cancellationToken: token).SuppressCancellationThrow();

        if (!isCanceled)
        {
            RunAction_Impl(target);
        }
    }

    protected abstract void RunAction_Impl(BattleUnit target);

    public virtual void Release()
    {
        ActionParameterPool.Release(this);
    }
}

public static class ActionParameterPool
{
    private static List<ActionParameter>[] _actionParameters;

    public static void Init()
    {
        int loopCount = (int)ActionParameterType.Length;
        _actionParameters = new List<ActionParameter>[loopCount];

        for (int i = 0; i < loopCount; i++)
        {
            _actionParameters[i] = new List<ActionParameter>(32);
        }
    }

    public static ActionParameter GetActionParameter(ActionParameterType type)
    {
        List<ActionParameter> list = _actionParameters[(int)type];

        ActionParameter actionParameter = null;
        if (list.Count > 0)
        {
            int index = list.Count - 1;
            actionParameter = list[index];
            list.RemoveAt(index);
        }
        else
        {
            actionParameter = CreateActionParameter(type);
        }
        Debug.Assert(actionParameter != null);
        return actionParameter;
    }

    private static ActionParameter CreateActionParameter(ActionParameterType type)
    {
        switch (type)
        {
            case ActionParameterType.Projectile:

                break;
        }

        return null;
    }

    public static void Release(ActionParameter actionParameter)
    {
        ActionParameterType type = actionParameter.ActionParameterType;
        List<ActionParameter> list = _actionParameters[(int)type];
        list.Add(actionParameter);
    }

    //TODO: Pool Release 기능 필요
}