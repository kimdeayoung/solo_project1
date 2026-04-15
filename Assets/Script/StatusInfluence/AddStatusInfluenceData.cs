using UnityEngine;

public struct AddStatusInfluenceData
{
    public AddStatusInfluenceType addStatusInfluenceType;

    public StatusInfluenceType statusInfluenceType;

    public float duration;

    public float value;

    public AddStatusInfluenceData(AddStatusInfluenceType addStatusInfluenceType, StatusInfluenceType statusInfluenceType, float duration, float value)
    {
        this.addStatusInfluenceType = addStatusInfluenceType;
        this.statusInfluenceType = statusInfluenceType;
        this.duration = duration;
        this.value = value;
    }

    public AddStatusInfluenceData(ApplyStatusInfluenceParameterSO parameter) : this(parameter.AddStatusInfluenceType, parameter.StatusInfluenceType, parameter.StatusInfluenceDuration, parameter.Value)
    {
    }
}
