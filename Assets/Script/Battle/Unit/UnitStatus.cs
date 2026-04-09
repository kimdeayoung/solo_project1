using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class UnitStatus
{
    protected BattleUnit owner;

    protected int hp;
    protected int maxHp;

    protected float atk;
    protected float def;

    protected StatusInfluenceInfo influenceInfo;

    protected int stunCount;

    public float MoveSpeed { get; protected set; }
    public float Haste { get; protected set; }

    public UnitStatus(BattleUnit owner, UnitStat stat)
    {
        this.owner = owner;
        influenceInfo = new StatusInfluenceInfo(owner);

        ApplyStat(stat);
    }

    protected virtual void ApplyStat(UnitStat stat)
    {
        maxHp = hp = stat.Hp;
        atk = stat.Atk;
        def = stat.Def;

        MoveSpeed = stat.MoveSpeed;
        Haste = 0;
        stunCount = 0;
    }

    public void OnUpdate(float deltaTime)
    {
        Assert.IsNotNull(influenceInfo);
        influenceInfo.OnUpdate(deltaTime);
    }

    public bool IsAlive()
    {
        return hp > 0;
    }

    public virtual UnitState GetUnitState()
    {
        return UnitState.Idle;
    }

    public void IncreaseStunCount()
    {
        if (stunCount++ == 0)
        {
            owner.BehaviourController.SetBehaviourState(UnitState.Stun);
        }
    }

    public void DecreaseStunCount()
    {
        if (--stunCount == 0)
        {
            owner.BehaviourController.SetBehaviourState(UnitState.Idle);
        }
    }
}
