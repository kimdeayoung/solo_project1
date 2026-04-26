using UnityEngine;

namespace PlayerState
{
    public class DeadState : BehaviourState
    {
        public override UnitState UnitState => UnitState.Dead;

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

