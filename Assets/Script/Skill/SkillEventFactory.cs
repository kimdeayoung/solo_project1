using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public static class SkillEventFactory
{
    public static SkillEvent CreateSkillEvent(TRSkillEvent trSkillEvent)
    {
        switch (trSkillEvent.EventType)
        {
            case EventType.NormalAttack:
                return new SkillEvent_NormalAttack(trSkillEvent);
            default:
                Assert.IsTrue(false);
                break;
        }

        return null;
    }
}
