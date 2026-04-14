using UnityEngine;

[CreateAssetMenu(fileName = "GlobalVariables", menuName = "Scriptable Objects/GlobalVariables")]
public class GlobalVariables : ScriptableObject
{
    [SerializeField] private Vector3 followCamOffset;
    public Vector3 FollowCamOffset => followCamOffset;

    [SerializeField] private bool followCamLookAtTarget;
    public bool FollowCamLookAtTarget => followCamLookAtTarget;
}
