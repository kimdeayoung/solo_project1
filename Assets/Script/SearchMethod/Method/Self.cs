using System.Collections.Generic;
using UnityEngine;

public class Self : SearchMethod
{
    public override SearchMethodType Type => SearchMethodType.Self;

    public override void ResetVariables(SearchMethodProperty property)
    {
    }

    public override WorldObject Run(WorldObject caster)
    {
        return caster;
    }

    public override WorldObject Run(WorldObject caster, HashSet<int> ignoreInstanceID)
    {
        return caster;
    }

    public override void Run(WorldObject caster, List<WorldObject> results)
    {
        results.Add(caster);
    }

    public override void Run(WorldObject caster, HashSet<int> ignoreInstanceID, List<WorldObject> results)
    {
        results.Add(caster);
    }
}
