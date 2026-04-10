using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public struct BtnUIData
{
    public string imageName;
    
    public Action onClickBtn;
}

public class ActionBtn : MonoBehaviour
{
    [SerializeField]
    private Image btnImage;

    [SerializeField]
    private Image cooltimeCover;

    private Action onClickBtn;

    public void Init(BtnUIData uiData)
    {
        btnImage.SetAtlasSprite(uiData.imageName);
        onClickBtn = uiData.onClickBtn;
    }

    public void OnClickBtn()
    {
        onClickBtn?.Invoke();
    }
}
