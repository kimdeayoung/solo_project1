using UnityEngine;

[CreateAssetMenu(fileName = "RandomActionDataSO", menuName = "Scriptable Objects/ActionData/RandomActionDataSO")]
public class RandomActionDataSO : BaseActionDataSO
{
    public override ActionDataType ActionType => ActionDataType.Random;

    public int[] _actionWeights;
    public int[] ActionWeights => _actionWeights;
}
