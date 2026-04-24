using UnityEngine;

namespace EnemyState
{
    public class IdleState : BehaviourState
    {
        private Enemy enemy;

        public override UnitState UnitState => UnitState.Idle;

        public override void Init(BattleUnit owner)
        {
            base.Init(owner);

            enemy = owner as Enemy;
            Debug.Assert(enemy != null);
        }

        public override void OnStart()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
            enemy.ChaseTarget();
            ownerBase.UpdateActionDatas(deltaTime);
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
        }

        public override void OnEnd()
        {
        }
    }
}

