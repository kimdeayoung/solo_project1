using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit
{
    protected uint hp;
    protected uint atk;
    protected uint def;
    protected float moveSpeed;

    protected Skill[] skills;

    protected UnitType unitType;

    public UnitType UnitType { get => unitType; }

    public Unit(TRUnit trUnit)
    {

    }

    
}
