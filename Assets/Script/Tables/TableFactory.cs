using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TableFactory
{
    public static TRFoundation CreateTRFoundation(TableType tableType)
    {
        TRFoundation trFoundation = null;
        if (tableType == TableType.Length)
        {
            return null;
        }

        switch (tableType)
        {
            case TableType.Skill:
                trFoundation = new TRSkill();
                break;
            case TableType.SkillEvent:
                trFoundation = new TRSkillEvent();
                break;
            case TableType.Stage:
                trFoundation = new TRStage();
                break;
            default:
                Assert.IsTrue(false);
                break;
        }

        return trFoundation;
    }
}
