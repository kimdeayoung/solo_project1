using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BattleUIController
{
    private HUD uiBattle;

    public Joystick Joystick { get => uiBattle.JoyStick; }
    public ActionBtn[] ActionBtns { get => uiBattle.ActionBtns; }

    public BattleUIController()
    {
    }

    public void CreateBattleUI(Action onFinished)
    {
        //AddressableBundleLoader.Instance.InstantiateAsync("BattleUI", null, (GameObject obj)=>
        //{
        //    Assert.IsNotNull(obj);
        //
        //    uiBattle = obj.GetComponent<BattleUI>();
        //    onFinished?.Invoke();
        //});
    }

    public void OnDestroy()
    {
        AddressableBundleLoader.Instance.DestroyInstantiateObj(uiBattle.gameObject);
    }
}
