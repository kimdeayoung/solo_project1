using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit
{
    protected TRUnit trUnit;
    protected Skill[] skills;

    public UnitType UnitType { get => trUnit.UnitType; }
    public uint Hp { get => trUnit.Hp; }
    public uint Def { get => trUnit.Def; }
    public float MoveSpeed { get => trUnit.MoveSpeed; }

    public string PrefabName { get =>  trUnit.PrefabName; }

    public Unit(TRUnit trUnit)
    {
        this.trUnit = trUnit;
        for (int i = 0; i < trUnit.SkillIndexes.Length; ++i)
        {
            skills[i] = new Skill(Tables.Instance.GetRecordOrNull<TRSkill>(TableType.Skill, trUnit.SkillIndexes[i]));
        }
    }
}
