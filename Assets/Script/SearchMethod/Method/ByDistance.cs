using System.Collections.Generic;
using UnityEngine;

public class ByDistance : SearchMethod
{
    private float range;
    private LayerMask layer;

    public override SearchMethodType Type => SearchMethodType.ByDistance;

    public override void ResetVariables(SearchMethodProperty property)
    {
        range = property.range;
        layer = property.layer;
    }

    public override WorldObject Run(WorldObject caster)
    {
        return SearchMethodFuncs.SearchWorldObjectOrNull(caster.transform.position, layer, range, SearchMethodFuncs.NearComparer);
    }

    public override WorldObject Run(WorldObject caster, HashSet<int> ignoreInstanceID)
    {
        return SearchMethodFuncs.SearchWorldObjectOrNull(caster.transform.position, layer, range, ignoreInstanceID, SearchMethodFuncs.NearComparer);
    }

    public override void Run(WorldObject caster, List<WorldObject> results)
    {
        WorldObject result = SearchMethodFuncs.SearchWorldObjectOrNull(caster.transform.position, layer, range, SearchMethodFuncs.NearComparer);
        results.Add(result);
    }

    public override void Run(WorldObject caster, HashSet<int> ignoreInstanceID, List<WorldObject> results)
    {
        WorldObject result = SearchMethodFuncs.SearchWorldObjectOrNull(caster.transform.position, layer, range, ignoreInstanceID, SearchMethodFuncs.NearComparer);
        results.Add(result);
    }
}
