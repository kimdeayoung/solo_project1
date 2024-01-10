using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SkillEvent_CreateProjectile : SkillEvent
{
    public SkillEvent_CreateProjectile(TRSkillEvent trSkillEvent) : base(trSkillEvent)
    {

    }

    public override void ExcuteEvent(BattleUnit caster, BattleUnit[] targetUnits)
    {
        PreCreationObjectType objectType = (PreCreationObjectType)integerValues[0];
        PreCreationObject obj = ObjectPoolManager.Instance.GetUnactiveObject(objectType);

        switch (objectType)
        {
            default:
                Assert.IsTrue(false);
                break;
        }
    }
}
