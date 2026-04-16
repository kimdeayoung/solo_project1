using System.Collections.Generic;
using UnityEngine;

public class InRange : SearchMethod
{
    private float range;
    private LayerMask layer;

    public override SearchMethodType Type => SearchMethodType.InRange;

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
        SearchMethodFuncs.SearchWorldObjects(caster.transform.position, layer, range, null, results);
    }

    public override void Run(WorldObject caster, HashSet<int> ignoreInstanceID, List<WorldObject> results)
    {
        SearchMethodFuncs.SearchWorldObjects(caster.transform.position, layer, range, ignoreInstanceID, results);
    }
}
