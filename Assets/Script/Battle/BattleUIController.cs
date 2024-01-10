using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BattleUIController
{
    private BattleController controller;

    private BattleUI uiBattle;
    private LoadState battleUILoadState;

    public LoadState BattleUILoadState { get => battleUILoadState; }

    public BattleUIController(BattleController controller, TRStage trStage)
    {
        this.controller = controller;
        battleUILoadState = LoadState.NotComplete;
    }

    public void CreateBattleUI()
    {
        AddressableBundleLoader.Instance.InstantiateAsync("BattleUI", null, OnFinishedCreateBattleUI);
    }

    private void OnFinishedCreateBattleUI(GameObject obj)
    {
        Assert.IsNotNull(obj);

        uiBattle = obj.GetComponent<BattleUI>();
        controller.UserControllerData = new UserControllerData(uiBattle.JoyStick);

        battleUILoadState = LoadState.Complete;
    }

    public void OnDestroy()
    {
        AddressableBundleLoader.Instance.DestroyInstantiateObj(uiBattle.gameObject);
    }
}
