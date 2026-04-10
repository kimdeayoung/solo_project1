using System;
using UnityEngine;
using System.Collections.Generic;

public class BattleLoadProcess : PreSceneLoadProcess
{
    public BattleLoadProcess(Action onLoadEndAction) : base(onLoadEndAction)
    {
        singleAssetNames = new List<string>();
        singleAssetNames.Add("Player");
    }
}
