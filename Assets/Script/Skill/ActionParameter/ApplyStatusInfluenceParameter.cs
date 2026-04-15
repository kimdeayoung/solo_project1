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

    protected override void RunAction_Impl(BattleUnit target)
    {
        target.Status.ApplyStatusInfluence(addStatusInfluenceData);
    }
}
