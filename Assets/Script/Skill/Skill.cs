using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Skill
{
    private uint index;

    private SkillType type;

    private SkillEvent[] events;
    private float activeCoolTime;

    public Skill(TRSkill trSkill)
    {
        index = trSkill.Index;
        type = trSkill.SkillType;
        events = new SkillEvent[trSkill.SkillEventIndexes.Length];
        for (int i = 0; i < trSkill.SkillEventIndexes.Length; ++i)
        {
            TRSkillEvent trSkillEvent = Tables.Instance.GetRecordOrNull<TRSkillEvent>(TableType.SkillEvent, trSkill.SkillEventIndexes[i]);
            Assert.IsNotNull(trSkillEvent);
            
            SkillEvent skillEvent = SkillEventFactory.CreateSkillEvent(trSkillEvent);
            skillEvent.BindIntegerValues(trSkill.EventIntegerValues[i]);
            skillEvent.BindFloatValues(trSkill.EventFloatValues[i]);
            skillEvent.BindStringValues(trSkill.EventStringValues[i]);
            
            events[i] = skillEvent;
        }

        activeCoolTime = trSkill.ActiveCoolTime;
    }

    public SkillType Type { get => type; }
    public float ActiveCoolTime { get => activeCoolTime; }
    public SkillEvent[] Events { get => events; }


}

