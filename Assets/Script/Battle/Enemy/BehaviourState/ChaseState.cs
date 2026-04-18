using UnityEngine;

namespace EnemyState
{
    public class ChaseState : BehaviourState
    {
        private Enemy enemy;

        public override UnitState UnitState => UnitState.Chase;

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
            Vector3 targetDir = (enemy.Target.transform.position - enemy.transform.position).normalized;
            ownerBase.SetMoveDirection(targetDir, 1.0f);

            ownerBase.UpdateActionDatas(deltaTime);
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            ownerBase.TranslateWithRotation(fixedDeltaTime);
        }

        public override void OnEnd()
        {
        }
    }
}

