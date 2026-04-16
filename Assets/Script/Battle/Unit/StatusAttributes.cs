using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StatusAttributes
{
    public int Hp { get; private set; }
    public int MaxHp { get; private set; }

    public float Atk { get; private set; }
    public float Def { get; private set; }

    public int StunCount { get; private set; }

    private ClampValuePair<float, float> moveSpeed;
    public float MoveSpeed => moveSpeed.ClampValue;

    public ClampValuePair<float, float> haste;
    public float Haste => haste.ClampValue;

    public virtual void ApplyStat(UnitStat stat)
    {
        MaxHp = Hp = stat.Hp;
        Atk = stat.Atk;
        Def = stat.Def;

        moveSpeed = new ClampValuePair<float, float>(stat.MoveSpeed, stat.MoveSpeed);
        haste = new ClampValuePair<float, float>();
        StunCount = 0;
    }

    public bool IsAlive()
    {
        return Hp > 0;
    }

    public void ChangeMoveSpeed(float value)
    {
        float computeVal = moveSpeed.OrigianlValue + value;
        float clampVal = computeVal;

        UnitStatusGlobalVariables variables = GameManager.Instance.GlobalVariables.UnitStatusGlobalVariables;
        if (clampVal < variables.MinMoveSpeed)
        {
            clampVal = variables.MinMoveSpeed;
        }
        else if (clampVal > variables.MaxMoveSpeed)
        {
            clampVal = variables.MaxMoveSpeed;
        }

        moveSpeed = new ClampValuePair<float, float>(computeVal, clampVal);
    }

    public void IncreaseStunCount()
    {
        ++StunCount;
    }

    public void DecreaseStunCount()
    {
        --StunCount;
    }
}
