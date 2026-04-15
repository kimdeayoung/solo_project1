using UnityEngine;

[CreateAssetMenu(fileName = "ApplyStatusInfluenceParameter", menuName = "Scriptable Objects/ActionParameter/ApplyStatusInfluence")]
public class ApplyStatusInfluenceParameterSO : ActionParameterSO
{
    [SerializeField] private StatusInfluenceType influenceType;
    public StatusInfluenceType StatusInfluenceType => influenceType;

    [SerializeField] private AddStatusInfluenceType addStatusInfluenceType;
    public AddStatusInfluenceType AddStatusInfluenceType => addStatusInfluenceType;

    [SerializeField] private float statusInfluenceDuration;
    public float StatusInfluenceDuration => statusInfluenceDuration;

    [SerializeField] private float value;
    public float Value => value;

    public override ActionParameterType ActionParameterType => ActionParameterType.ApplyStatusInfluence;
}
