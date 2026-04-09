using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleUnit : MonoBehaviour
{
    public ActionResourceType ResourceType { get; protected set; }
    public int ActionResource { get; protected set; }

    public UnitStatus Status { get; protected set; }
    public IBehaviourController BehaviourController { get; private set; }

    public virtual void Init()
    {
        BehaviourController = new BehaviourController();
    }

    public abstract void OnUpdate(float deltaTime);

    public bool IsAlive()
    {
        return Status.IsAlive();
    }

    public virtual bool HasEnoughActionResource(ActionResourceData data)
    {
        if (data.resourceType == ResourceType)
        {
            return ActionResource >= data.actionResource;
        }

        return false;
    }

    public virtual void UseActionResource(ActionResourceData data)
    {
        Debug.Assert(HasEnoughActionResource(data));

        ActionResource -= data.actionResource;
    }

    public virtual void UpdateActionDatas(float deltaTime)
    {
    }

    public void RunAnimation(string animationName)
    {

    }
}
