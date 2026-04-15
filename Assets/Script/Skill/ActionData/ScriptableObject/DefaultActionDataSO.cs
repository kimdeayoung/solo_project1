using UnityEngine;

[CreateAssetMenu(fileName = "DefaultActionDataSO", menuName = "Scriptable Objects/ActionData/DefaultActionDataSO")]
public class DefaultActionDataSO : BaseActionDataSO
{
    public override ActionDataType ActionType => ActionDataType.Default;
}
