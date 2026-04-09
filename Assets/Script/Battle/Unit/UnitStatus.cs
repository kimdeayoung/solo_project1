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

    protected float moveSpeed;

    protected StatusInfluenceInfo influenceInfo;

    public float MoveSpeed { get => moveSpeed; }

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

        moveSpeed = stat.MoveSpeed;
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
}
