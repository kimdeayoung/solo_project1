using System.Collections.Generic;
using UnityEngine;

public abstract class BattleUnit : WorldObject
{
    public abstract BattleUnitType Type { get; }
    public ActionResourceType ResourceType { get; protected set; }
    public int ActionResource { get; protected set; }

    public string AssetName { get; private set; }

    public abstract float AccelerationRatio { get; }

    public IBehaviourController BehaviourController { get; private set; }

    protected List<BaseActionData> actionDatas;
    public IReadOnlyList<BaseActionData> ActionDatas => actionDatas;

    protected List<BaseActionData> collisionActions;

    [SerializeField] protected Animator animator;

    public override void Init(string assetName)
    {
        base.Init(assetName);
        BehaviourController = new BehaviourController();

        AssetName = assetName;
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

    protected void OnCollisionEnter(Collision collision)
    {
        WorldObject target = collision.gameObject.GetComponent<DetectableComponent>().WorldObject;

        if (IsHitAble(target, out int weightOffset))
        {
            HitParameter hitParameter = new HitParameter(Status.StatusAttributes, Status.StatusAttributes.CollisionDamageMultiplier, AccelerationRatio, weightOffset);
            target.OnHit(ref hitParameter);

            UnitStatusGlobalVariables values = GameManager.Instance.GlobalVariables.UnitStatusGlobalVariables;

            if (weightOffset > 0)
            {
                float duration = Random.Range(values.ApplyKnockbackDuration.x, values.ApplyKnockbackDuration.y);
                target.Status.ApplyStatusInfluence(this, new AddStatusInfluenceData(AddStatusInfluenceType.Unique, StatusInfluenceType.Knockback, duration, weightOffset));
            }

            if (collisionActions != null)
            {
                int loopCount = collisionActions.Count;
                for (int i = 0; i < loopCount; i++)
                {
                    TryExecuteAction(collisionActions[i]);
                }
            }

            BehaviourController.OnCollisionEnter(target);
        }
    }

    protected abstract bool IsHitAble(WorldObject target, out int weightOffset);

    public override void OnHit(ref HitParameter hitParameter)
    {
        base.OnHit(ref hitParameter);

        Status.RunOnHitStatusInfluence(ref hitParameter);

        int damage = hitParameter.GetDamage();
        if (damage > 0)
        {
            Status.StatusAttributes.ApplyDamage(damage, out int trueDamage);
            Debug.Log($"onDamage: {trueDamage}");
            //TODO: Display damage font(trueDamage)
        }
    }

    public override void OnHeal(int value)
    {
        base.OnHeal(value);

        float heal = value * Status.StatusAttributes.HealMultiplier;
        if (heal <= 0.0f)
        {
            return;
        }

        Status.StatusAttributes.ApplyHeal(Mathf.CeilToInt(heal), out int trueHeal);
        //TODO: Display heal font(trueDamage)
    }
}
