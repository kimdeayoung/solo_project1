using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DetectableComponent : MonoBehaviour
{
    [SerializeField] private WorldObject worldObject;
    public WorldObject WorldObject => worldObject;
}
