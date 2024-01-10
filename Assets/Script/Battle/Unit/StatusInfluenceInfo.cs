using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StatusInfluenceInfo
{
    private BattleUnit owner;


    private float[] applyCrowdControlTimes;

    public StatusInfluenceInfo(BattleUnit owner)
    {
        this.owner = owner;


        applyCrowdControlTimes = new float[(int)CrowdControlType.Length];
    }

    public void Update()
    {


        for (int i = 0; i < applyCrowdControlTimes.Length; ++i)
        {
            if (applyCrowdControlTimes[i] > 0.0f)
            {
                applyCrowdControlTimes[i] -= Time.deltaTime;
                if (applyCrowdControlTimes[i] <= 0.0f)
                {
                    applyCrowdControlTimes[i] = 0.0f;
                }
            }
        }
    }

    public void ApplyCrowdControl(CrowdControlType type, float time)
    {
        Assert.IsFalse(type == CrowdControlType.Length);

        int typeIndex = (int)type;
        if (applyCrowdControlTimes[typeIndex] > time)
        {
            return;
        }

        applyCrowdControlTimes[typeIndex] = time;
    }

    public bool IsApplyCrowdControl(CrowdControlType type)
    {
        return applyCrowdControlTimes[(int)type] > 0.0f;
    }
}
