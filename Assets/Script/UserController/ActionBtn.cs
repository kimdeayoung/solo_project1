using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ActionBtn : MonoBehaviour
{
    [SerializeField]
    private Image btnImage;

    [SerializeField]
    private Image cooltimeCover;

    private WorldObject caster;
    private BaseActionData actionData;

    public void Init(WorldObject caster, BaseActionData actionData)
    {
        btnImage.SetSprite(actionData.IconName);

        this.actionData = actionData;
    }

    //private void Update()
    //{
    //    //TODO: cooltimeCover 작동 기능 추가
    //}

    public void OnClickBtn()
    {
        if (actionData.IsExecuteAble() && actionData.SearchActionTarget())
        {
            actionData.Execute(caster).Forget();
        }
    }
}
