using UnityEngine;

namespace Projectile
{
    public interface IProjectileBehaviour
    {
        void ResetVariables();
        void Release();
    }

    public interface IUpdateBehaviour : IProjectileBehaviour
    {
        void Update(float deltaTime);
    }

    public interface IFixedUpdateBehaviour : IProjectileBehaviour
    {
        void FixedUpdate(float fixedDeltaTime);
    }

    public interface ITriggerBehiavour : IProjectileBehaviour
    {
        void OnTriggerEnter(Collider other);
    }
}
