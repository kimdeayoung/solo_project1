using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class UnitStatus
{
    protected BattleUnit owner;
    private StatusAttributes statusAttributes;

    protected StatusInfluenceInfo influenceInfo;

    public UnitStatus(BattleUnit owner, UnitStat stat)
    {
        this.owner = owner;
        influenceInfo = new StatusInfluenceInfo(owner);

        statusAttributes = new StatusAttributes();
        statusAttributes.ApplyStat(stat);
    }

    public void OnUpdate(float deltaTime)
    {
        Assert.IsNotNull(influenceInfo);
        influenceInfo.OnUpdate(deltaTime);
    }

    public bool IsAlive()
    {
        return statusAttributes.IsAlive();
    }

    public void ApplyStatusInfluence(AddStatusInfluenceData data)
    {
        if (!IsAlive())
        {
            return;
        }

        Debug.Assert(influenceInfo != null);
        influenceInfo.ApplyStatusInfluence(data);
    }

    public void ChangeMoveSpeed(float value)
    {
        statusAttributes.ChangeMoveSpeed(value);
    }

    public float GetHaste()
    {
        return statusAttributes.Haste;
    }

    public void IncreaseStunCount()
    {
        if (statusAttributes.StunCount == 0)
        {
            owner.BehaviourController.SetBehaviourState(UnitState.Stun);
        }
        statusAttributes.IncreaseStunCount();
    }

    public void DecreaseStunCount()
    {
        statusAttributes.DecreaseStunCount();
        if (statusAttributes.StunCount == 0)
        {
            if (IsAlive())
            {
                owner.BehaviourController.SetBehaviourState(UnitState.Idle);
            }
        }
    }
}
