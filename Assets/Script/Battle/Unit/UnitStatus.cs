using UnityEngine;
using UnityEngine.Assertions;

public abstract class UnitStatus
{
    protected WorldObject owner;
    private StatusAttributes statusAttributes;
    public StatusAttributes StatusAttributes => statusAttributes;

    protected StatusInfluenceInfo influenceInfo;

    public UnitStatus(WorldObject owner, UnitStat stat)
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

    public virtual void ChangeMoveSpeed(float value)
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
            switch (owner)
            {
                case BattleUnit battleUnit:
                    battleUnit.BehaviourController.SetBehaviourState(UnitState.Stun);
                    break;
            }
        }
        statusAttributes.IncreaseStunCount();
    }

    public void DecreaseStunCount()
    {
        statusAttributes.DecreaseStunCount();
        if (!IsAlive())
        {
            return;
        }

        if (statusAttributes.StunCount == 0)
        {
            switch (owner)
            {
                case BattleUnit battleUnit:
                    battleUnit.BehaviourController.SetBehaviourState(UnitState.Idle);
                    break;
            }
        }
    }
}
