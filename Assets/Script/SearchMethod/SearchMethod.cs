using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SearchMethod
{
    public abstract SearchMethodType Type { get; }

    public abstract void ResetVariables(SearchMethodProperty property);

    public abstract WorldObject Run(WorldObject caster);
    public abstract WorldObject Run(WorldObject caster, HashSet<int> ignoreInstanceID);
    public abstract void Run(WorldObject caster, List<WorldObject> results);
    public abstract void Run(WorldObject caster, HashSet<int> ignoreInstanceID, List<WorldObject> results);
}

[Serializable]
public struct SearchMethodProperty
{
    public float range;
    public LayerMask layer;

    public SearchMethodProperty(float range, LayerMask layer)
    {
        this.range = range;
        this.layer = layer;
    }
}

public static class SearchMethodFuncs
{
    private static Collider[] colliders = new Collider[256];

    public static NearComparer NearComparer = new NearComparer();
    public static FarComparer FarComparer = new FarComparer();


    public static WorldObject SearchWorldObjectOrNull(Vector3 position, LayerMask layer, float range, IComparer<float> distanceComparer)
    {
        return SearchWorldObjectOrNull_Impl(position, layer, range, null, distanceComparer);
    }

    public static WorldObject SearchWorldObjectOrNull(Vector3 position, LayerMask layer, float range, HashSet<int> ignoreInstanceID, IComparer<float> distanceComparer)
    {
        return SearchWorldObjectOrNull_Impl(position, layer, range, ignoreInstanceID, distanceComparer);
    }

    private static WorldObject SearchWorldObjectOrNull_Impl(Vector3 position, LayerMask layer, float range, HashSet<int> ignoreInstanceID, IComparer<float> distanceComparer)
    {
        int count = Physics.OverlapSphereNonAlloc(position, range, colliders, layer);

        if (count == 0)
        {
            return null;
        }

        DetectableComponent detectableComponent = colliders[0].GetComponent<DetectableComponent>();
        Debug.Assert(detectableComponent != null);
        WorldObject result = detectableComponent.WorldObject;
        Debug.Assert(result != null);
        float sqrMagnitude = (position - result.transform.position).sqrMagnitude;

        for (int i = 1; i < count; i++)
        {
            detectableComponent = colliders[i].GetComponent<DetectableComponent>();
            Debug.Assert(detectableComponent != null);
            WorldObject worldObject = detectableComponent.WorldObject;
            Debug.Assert(worldObject != null);
            if (ignoreInstanceID != null && ignoreInstanceID.Contains(worldObject.GetInstanceID()))
            {
                continue;
            }

            float compareValue = (position - worldObject.transform.position).sqrMagnitude;
            if (distanceComparer.Compare(sqrMagnitude, compareValue) < 0)
            {
                result = worldObject;
                sqrMagnitude = compareValue;
            }
        }

        return result;
    }

    public static void SearchWorldObjects(Vector3 position, LayerMask layer, float range, HashSet<int> ignoreInstanceID, List<WorldObject> result)
    {
        int count = Physics.OverlapSphereNonAlloc(position, range, colliders, layer);

        if (count == 0)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            DetectableComponent detectableComponent = colliders[0].GetComponent<DetectableComponent>();
            WorldObject worldObject = detectableComponent.WorldObject;

            if (ignoreInstanceID != null && ignoreInstanceID.Contains(worldObject.GetInstanceID()))
            {
                continue;
            }

            result.Add(worldObject);
        }
    }
}

public class NearComparer : IComparer<float>
{
    public int Compare(float x, float y)
    {
        return x < y ? -1 : 1;
    }
}

public class FarComparer : IComparer<float>
{
    public int Compare(float x, float y)
    {
        return x < y ? 1 : -1;
    }
}