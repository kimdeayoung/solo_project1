using UnityEngine;

public class ApplyStatusParameter : ActionParameter
{
    public override ActionParameterType ActionParameterType => ActionParameterType.ApplyStatus;

    public override void ResetVariables(ActionParameterSO data)
    {
        ApplyStatusParameterSO applyStatus = data as ApplyStatusParameterSO;
    }

    protected override void RunAction_Impl(BattleUnit target)
    {

    }
}
