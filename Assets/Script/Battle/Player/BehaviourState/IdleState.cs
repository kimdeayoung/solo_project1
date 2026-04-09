using UnityEngine;

namespace PlayerState
{
    public class IdleState : BehaviourState
    {
        public override UnitState UnitState => UnitState.Idle;

        public override void OnStart()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnEnd()
        {

        }
    }
}

