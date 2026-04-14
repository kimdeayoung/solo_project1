using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    private GameObject target;
    private Vector3 offset;
    private bool lookAtTarget;

    public void ResetVariables(GameObject target, Vector3 offset, bool lookAtTarget)
    {
        ResetVariables_Impl(target, offset, Vector3.zero, lookAtTarget);
    }

    public void ResetVariables(GameObject target, Vector3 offset, Vector3 rotation)
    {
        ResetVariables_Impl(target, offset, rotation, false);
    }

    private void ResetVariables_Impl(GameObject target, Vector3 offset, Vector3 rotation, bool lookAtTarget)
    {
        this.target = target;
        this.offset = offset;

        transform.position = target.transform.position + offset;
        this.lookAtTarget = lookAtTarget;
        if (!lookAtTarget)
        {
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

    private void Update()
    {
        transform.position = target.transform.position + offset;
        if (lookAtTarget)
        {
            transform.LookAt(target.transform);
        }
    }
}
