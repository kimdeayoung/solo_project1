using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class SkillEvent
{
    protected EventType eventType;

    protected int[] integerValues;
    protected float[] floatValues;
    protected string[] stringValues;

    public EventType EventType { get => eventType; }

    public SkillEvent(TRSkillEvent trSkillEvent)
    {
        eventType = trSkillEvent.EventType;
    }

    public void BindIntegerValues(int[] values)
    {
        integerValues = values;
    }

    public void BindFloatValues(float[] values)
    {
        floatValues = values;
    }

    public void BindStringValues(string[] values)
    {
        stringValues = values;
    }

    public abstract void ExcuteEvent(BattleUnit caster, BattleUnit[] targetUnits);

}