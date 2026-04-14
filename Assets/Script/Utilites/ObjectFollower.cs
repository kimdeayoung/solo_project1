using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    private GameObject target;
    private Vector3 offset;

    public void ResetVariables(GameObject target, Vector3 offset, bool lookAtTarget)
    {
        this.target = target;
        this.offset = offset;

        if (lookAtTarget)
        {
            transform.LookAt(target.transform);
        }
    }

    private void Update()
    {
        Vector3 newPosition = target.transform.position + offset;
        transform.position = newPosition;
    }
}
