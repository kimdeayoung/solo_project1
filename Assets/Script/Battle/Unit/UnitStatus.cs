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
        Debug.Assert(influenceInfo != null);
        influenceInfo.OnUpdate(deltaTime);
    }

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        Debug.Assert(influenceInfo != null);
        influenceInfo.OnFixedUpdate(fixedDeltaTime);
    }

    public bool IsAlive()
    {
        return statusAttributes.IsAlive();
    }

    public void ApplyStatusInfluence(WorldObject caster, AddStatusInfluenceData data)
    {
        if (!IsAlive())
        {
            return;
        }

        Debug.Assert(influenceInfo != null);
        influenceInfo.ApplyStatusInfluence(caster, data);
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

    public void SetKnockbackState(bool knockback)
    {
        if (statusAttributes.TrySetKnockbackState(knockback))
        {
            if (knockback)
            {
                IncreaseStunCount();
            }
            else
            {
                DecreaseStunCount();
            }
        }
    }
}
