using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSkill
{
    private BattleUnit owner;

    private Skill skill;
    private float currentCoolTime;

    public BattleSkill(BattleUnit owner, Skill skill)
    {
        this.owner = owner;
        this.skill = skill;
    }

    public void Update()
    {

    }

    public void ExcuteSkill(BattleUnit target)
    {
        BattleUnit[] targets = new BattleUnit[] { target };
        ExcuteSkill(targets);
    }

    public void ExcuteSkill(BattleUnit[] targets)
    {
        if (IsUseAble())
        {
            currentCoolTime = 0.0f;
        }
    }

    public bool IsUseAble()
    {
        return currentCoolTime >= skill.ActiveCoolTime;
    }
}
