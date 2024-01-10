using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : ManagerWithMono<BattleManager>
{
    BattleController controller;

    public BattleController Controller { get => controller; }

    public void StartBattle(BattleStartData battleStartData)
    {
        AddressableBundleLoader.Instance.LoadSceneAsync(battleStartData.trStage.SceneName, onFinished: () =>
        {
            controller = new BattleController(battleStartData);
            controller.CrateBattleObjects();
        });
    }

    public void Update()
    {
        if (controller != null)
        {
            controller.Update();
        }
    }

    public override void ClearData()
    {
        controller.ClearData();
        controller = null;
    }
}
