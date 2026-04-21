using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class ActionParameter
{
    public abstract ActionParameterType ActionParameterType { get; }

    public int ParameterUniqueID { get; private set; }
    public float RunDelay { get; private set; }
    public float Duration { get; private set; }
    public string AnimationName { get; private set; }

    public virtual void ResetVariables(ActionParameterSO data)
    {
        ParameterUniqueID = data.ParameterUniqueID;
        RunDelay = data.RunDelay;

        Duration = data.ActionDuration;

        AnimationName = data.AnimationName;
    }

    public async void RunAction(WorldObject caster, List<WorldObject> target, CancellationToken token)
    {
        if (RunDelay > 0)
        {
            bool isCanceled = await UniTask.WaitForSeconds(RunDelay, cancellationToken: token).SuppressCancellationThrow();

            if (!isCanceled)
            {
                RunAction_Impl(caster, target);
            }
        }
        else
        {
            RunAction_Impl(caster, target);
        }
    }

    protected abstract void RunAction_Impl(WorldObject caster, List<WorldObject> targets);

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

    public static ActionParameter GetActionParameter(ActionParameterSO data)
    {
        List<ActionParameter> list = _actionParameters[(int)data.ActionParameterType];

        ActionParameter actionParameter = null;
        if (list.Count > 0)
        {
            int index = list.Count - 1;
            actionParameter = list[index];
            list.RemoveAt(index);
        }
        else
        {
            actionParameter = CreateActionParameter(data.ActionParameterType);
        }
        actionParameter.ResetVariables(data);
        Debug.Assert(actionParameter != null);
        return actionParameter;
    }

    private static ActionParameter CreateActionParameter(ActionParameterType type)
    {
        switch (type)
        {
            case ActionParameterType.Projectile:
                return new ProjectileParameter();

            case ActionParameterType.ApplyStatusInfluence:
                return new ApplyStatusInfluenceParameter();

            case ActionParameterType.MeleeHit:
                return new MeleeHitParameter();
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