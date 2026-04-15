using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RandomActionData : BaseActionData
{
    private List<ActionParameter> _actionParameters = new List<ActionParameter>(8);
    private List<int> _actionWeights = new List<int>(8);

    public override ActionDataType ActionDataType => ActionDataType.Random;

    public override void ResetVariables(BattleUnit owner, BaseActionDataSO data)
    {
        base.ResetVariables(owner, data);

        RandomActionDataSO randomAction = data as RandomActionDataSO;
        Debug.Assert(randomAction != null);

        ActionParameterSO[] actionParameters = randomAction.ActionParameters;
        int actionParameterCount = actionParameters.Length;
        for (int i = 0; i < actionParameterCount; i++)
        {
            _actionParameters.Add(ActionParameterPool.GetActionParameter(actionParameters[i]));
        }

        int[] actionWeights = randomAction.ActionWeights;
        int weights = actionWeights.Length;
        for (int i = 0; i < weights; i++)
        {
            _actionWeights.Add(actionWeights[i]);
        }

        Debug.Assert(_actionParameters.Count == _actionWeights.Count);
    }

    protected override float RunActions()
    {
        float duration = 0.0f;
        int actionWeights = _actionWeights.Count;

        int totalWeights = 0;
        for (int i = 0; i < actionWeights; i++)
        {
            totalWeights += _actionWeights[i];
        }

        int weight = Random.Range(0, totalWeights);

        int actionParameterCount = _actionParameters.Count;

        CancellationToken token = cancelToken.Token;
        for (int i = 0; i < actionParameterCount; i++)
        {
            weight -= _actionWeights[i];
            if (weight < 0)
            {
                ActionParameter actionParameter = _actionParameters[i];
                duration = actionParameter.RunDelay + actionParameter.Duration;

                actionParameter.RunAction(null, token);
                owner.RunAnimation(actionParameter.AnimationName);
            }
        }

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

        _actionWeights.Clear();
    }
}
