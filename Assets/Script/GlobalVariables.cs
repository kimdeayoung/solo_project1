using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalVariables", menuName = "Scriptable Objects/GlobalVariables")]
public class GlobalVariables : ScriptableObject
{
    [SerializeField] private Vector3 followCamOffset;
    public Vector3 FollowCamOffset => followCamOffset;

    [SerializeField] private Vector3 followCamRotation;
    public Vector3 FollowCamRotation => followCamRotation;

    [SerializeField] private UnitStatusGlobalVariables unitStatusGlobalVariables;
    public UnitStatusGlobalVariables UnitStatusGlobalVariables => unitStatusGlobalVariables;
}

[Serializable]
public class UnitStatusGlobalVariables
{
    [SerializeField] private float minMoveSpeed;
    public float MinMoveSpeed => minMoveSpeed;

    [SerializeField] private float maxMoveSpeed;
    public float MaxMoveSpeed => maxMoveSpeed;

    [SerializeField] private Vector2 applyKnockbackDuration;
    public Vector2 ApplyKnockbackDuration => applyKnockbackDuration;

    [SerializeField] private int applyKnockbackValue;
    public int ApplyKnockbackValue => applyKnockbackValue;
}