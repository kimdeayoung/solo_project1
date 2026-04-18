using UnityEngine;

namespace PlayerState
{
    public class IdleState : BehaviourState
    {
        private Player player;

        public override UnitState UnitState => UnitState.Idle;

        public override void Init(BattleUnit owner)
        {
            base.Init(owner);

            player = owner as Player;
            Debug.Assert(player != null);
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
            ownerBase.TranslateWithRotation(fixedDeltaTime);
        }

        public override void OnEnd()
        {
        }
    }
}

