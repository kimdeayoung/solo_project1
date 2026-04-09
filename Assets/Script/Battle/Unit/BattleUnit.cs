using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleUnit : MonoBehaviour
{
    protected UnitStatus status;

    public virtual void Init()
    {
    }

    public bool IsAlive()
    {
        return status.IsAlive();
    }

    public UnitState GetUnitState()
    {
        return status.GetUnitState();
    }
}
