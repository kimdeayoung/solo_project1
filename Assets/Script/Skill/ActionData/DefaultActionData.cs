using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DefaultActionData : BaseActionData
{
    private List<ActionParameter> _actionParameters = new List<ActionParameter>(8);

    public override ActionDataType ActionDataType => ActionDataType.Default;

    public override void ResetVariables(BattleUnit owner, BaseActionDataSO data)
    {
        base.ResetVariables(owner, data);

        DefaultActionDataSO defaultAction = data as DefaultActionDataSO;
        Debug.Assert(defaultAction != null);

        ActionParameterSO[] actionParameters = defaultAction.ActionParameters;
        int actionParameterCount = actionParameters.Length;
        for (int i = 0; i < actionParameterCount; i++)
        {
            _actionParameters.Add(ActionParameterPool.GetActionParameter(actionParameters[i]));
        }
    }

    protected override float RunActions(List<WorldObject> searchResult)
    {
        float duration = 0.0f;
        int actionParameterCount = _actionParameters.Count;

        CancellationToken token = cancelToken.Token;
        string animationName = string.Empty;
        for (int i = 0; i < actionParameterCount; i++)
        {
            ActionParameter actionParameter = _actionParameters[i];

            float newDuration = actionParameter.RunDelay + actionParameter.Duration;
            if (duration < newDuration)
            {
                duration = newDuration;
                animationName = actionParameter.AnimationName;
            }

            actionParameter.RunAction(searchResult, token);
        }
        owner.RunAnimation(animationName);

        return duration;
    }

    public override void Release()
    {
        base.Release();

        int actionParameterCount = _actionParameters.Count;
        for (int i = 0; i < actionParameterCount; i++)
        {
            _actionParameters[i].Release();
        }

        _actionParameters.Clear();
    }
}
