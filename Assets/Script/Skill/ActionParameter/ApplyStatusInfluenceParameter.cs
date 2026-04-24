using System.Collections.Generic;
using UnityEngine;

public class ApplyStatusInfluenceParameter : ActionParameter
{
    private AddStatusInfluenceData addStatusInfluenceData;

    public override ActionParameterType ActionParameterType => ActionParameterType.ApplyStatusInfluence;

    public override void ResetVariables(ActionParameterSO data)
    {
        ApplyStatusInfluenceParameterSO applyStatusInfluence = data as ApplyStatusInfluenceParameterSO;
        addStatusInfluenceData = new AddStatusInfluenceData(applyStatusInfluence);
    }

    protected override void RunAction_Impl(WorldObject caster, List<WorldObject> targets)
    {
        int loopCount = targets.Count;
        for (int i = 0; i < loopCount; i++)
        {
            targets[i].Status.ApplyStatusInfluence(caster, addStatusInfluenceData);
        }
    }
}
