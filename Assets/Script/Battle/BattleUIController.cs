using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BattleUIController
{
    private BattleController controller;

    private BattleUI uiBattle;

    public Joystick Joystick { get => uiBattle.JoyStick; }

    public BattleUIController(BattleController controller)
    {
        this.controller = controller;
    }

    public void CreateBattleUI(Action onFinished)
    {
        AddressableBundleLoader.Instance.InstantiateAsync("BattleUI", null, (GameObject obj)=>
        {
            Assert.IsNotNull(obj);

            uiBattle = obj.GetComponent<BattleUI>();
            onFinished?.Invoke();
        });
    }

    public void OnDestroy()
    {
        AddressableBundleLoader.Instance.DestroyInstantiateObj(uiBattle.gameObject);
    }
}
