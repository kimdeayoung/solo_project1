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
            ownerBase.UpdateActionDatas(deltaTime);
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            //TODO: 이동 로직 Nav Mesh 사용으로 변경
        }

        public override void OnEnd()
        {
        }
    }
}

