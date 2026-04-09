using UnityEngine;

namespace PlayerState
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

        public override void OnEnd()
        {

        }
    }
}

