using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BattleInit : BattleState
{
    private enum BattleLoadType
    {
        Player,
        BattleUI,
    }

    private BattleUIController battleUIController;

    private Dictionary<BattleLoadType, LoadState> loadStates;

    public BattleInit(BattleController battleController) : base(battleController)
    {
        battleUIController = battleController.BattleUIController;

        loadStates = new Dictionary<BattleLoadType, LoadState>();
        foreach(BattleLoadType loadType in Enum.GetValues(typeof(BattleLoadType)))
        {
            loadStates.Add(loadType, LoadState.NotComplete);
        }
    }

    public override void StateEnter()
    {
        //AddressableBundleLoader.Instance.LoadAssetAsync<GameObject>("BattleUI", CreateBattleUI);
        //AddressableBundleLoader.Instance.LoadAssetAsync<GameObject>("Player", null);
    }

    private void CreatePlayer()
    {
        AddressableBundleLoader.Instance.InstantiateAsync("Player", null, (GameObject obj) =>
        {
            Player player = obj.GetComponent<Player>();
            Debug.Assert(player != null);
            player.Init();

            controller.AddBattleUnit(player);

            loadStates[BattleLoadType.Player] = LoadState.Complete;
        });
    }

    private void CreateBattleUI(GameObject obj)
    {
        battleUIController.CreateBattleUI(OnFinishedCreateBattleUI);
    }

    public override void StateUpdate(float deltaTime)
    {
        bool isLoadComplete = true;
        foreach (BattleLoadType loadType in Enum.GetValues(typeof(BattleLoadType)))
        {
            if (loadStates[loadType] == LoadState.NotComplete)
            {
                isLoadComplete = false;
                break;
            }
        }

        if (isLoadComplete)
        {
            controller.SetBattleState(BattleStateType.Progress);
        }
    }

    public override void StateFixedUpdate(float fixedDeltaTime)
    {
    }

    public override void StateExit()
    {
    }

    private void OnFinishedCreateBattleUI()
    {
        CreatePlayer();
        loadStates[BattleLoadType.BattleUI] = LoadState.Complete;
    }
}
