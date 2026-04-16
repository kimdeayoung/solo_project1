using System;
using UnityEngine;
using System.Collections.Generic;

public class BattleLoadProcess : PreSceneLoadProcess
{
    public BattleLoadProcess(Action onLoadEndAction) : base(onLoadEndAction)
    {
        labelNames = new List<string>();
        labelNames.Add("IngamePrefab");
        labelNames.Add("IngameUI");

        spriteLabelNames = new List<string>();
        spriteLabelNames.Add("IngameSprite");
    }
}
