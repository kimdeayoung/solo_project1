using UnityEngine;

public enum BeginCommandConditionType
{
    None,
    Once,
}

public abstract class BeginCommandCondition
{
    public abstract bool IsBeginAble(CommandContextContainer contextContainer);
}
