using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
        AddressableBundleLoader.Instance.InstantiateAsync(controller.BattleStartData.playerCharacterInfo.PrefabName, null, (GameObject obj) =>
        {
            controller.Player = obj.GetComponent<BattleCharacter>();
            Assert.IsNotNull(controller.Player);

            loadStates[BattleLoadType.Player] = LoadState.Complete;
        });

        battleUIController.CreateBattleUI(OnFinishedCreateBattleUI);
    }

    public override void StateUpdate()
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

    public override void StateExit()
    {
        controller.Player.UserControllerData = new UserControllerData(battleUIController.Joystick);
    }

    private void OnFinishedCreateBattleUI()
    {
        loadStates[BattleLoadType.BattleUI] = LoadState.Complete;
    }
}
