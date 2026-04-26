using UnityEngine;

namespace EnemyState
{
    public class DeadState : BehaviourState
    {
        private Enemy enemy;

        public override UnitState UnitState => UnitState.Dead;

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
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
        }

        public override void OnEnd()
        {
        }
    }
}

