using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class Projectile : WorldObject
    {
        private List<IUpdateBehaviour> updateBehaviours = new List<IUpdateBehaviour>(8);
        private List<IFixedUpdateBehaviour> fixedUpdateBehaviours = new List<IFixedUpdateBehaviour>(8);
        private List<ITriggerBehiavour> triggerBehiavours = new List<ITriggerBehiavour>(8);

        public void ResetVariables()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            int loopCount = updateBehaviours.Count;
            for (int i = 0; i < loopCount; i++)
            {
                updateBehaviours[i].Update(deltaTime);
            }
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            int loopCount = fixedUpdateBehaviours.Count;
            for (int i = 0; i < loopCount; i++)
            {
                fixedUpdateBehaviours[i].FixedUpdate(fixedDeltaTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            int loopCount = triggerBehiavours.Count;
            for (int i = 0; i < loopCount; i++)
            {
                triggerBehiavours[i].OnTriggerEnter(other);
            }
        }
    }
}