using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleUnit : MonoBehaviour
{
    protected UnitStatus status;
    protected UnitStatus baseStatus;
    protected BattleSkill[] skills;

    public abstract void Init(Unit unit);

    public bool IsAlive()
    {
        return status.IsAlive();
    }
}
