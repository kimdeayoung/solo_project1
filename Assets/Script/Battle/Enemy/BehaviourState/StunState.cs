using UnityEngine;

namespace EnemyState
{
    public class StunState : BehaviourState
    {
        public override UnitState UnitState => UnitState.Stun;

        public override void OnStart()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
        }

        public override void OnCollisionEnter(WorldObject target)
        {
            if (ownerBase.Status.StatusAttributes.Knockback)
            {
                int targetLayer = target.gameObject.layer;
                if (targetLayer == LayerMaskCached.Player)
                {
                    return;
                }

                float atk = GameManager.Instance.GlobalVariables.UnitStatusGlobalVariables.KnockbackCollisionAtk;
                {
                    StatusAttributes statusAttributes = ownerBase.Status.StatusAttributes;
                    HitParameter parameter = new HitParameter(atk, statusAttributes.DefT0, statusAttributes.DefT1, 1.0f, 1.0f, 0);
                    ownerBase.OnHit(ref parameter);
                }
                

                if (targetLayer == LayerMaskCached.Enemy)
                {
                    StatusAttributes statusAttributes = target.Status.StatusAttributes;
                    HitParameter parameter = new HitParameter(atk, statusAttributes.DefT0, statusAttributes.DefT1, 1.0f, 1.0f, 0);
                    target.OnHit(ref parameter);
                }
                else if (targetLayer == LayerMaskCached.Obstacle)
                {
                    ownerBase.Status.SetKnockbackState(false);
                }
            }
        }

        public override void OnEnd()
        {
        }
    }
}

