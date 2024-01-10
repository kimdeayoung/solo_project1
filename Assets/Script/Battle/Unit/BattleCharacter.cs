using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BattleCharacter : BattleUnit
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float rotateSpeed;

    private UserControllerData userControllerData;

    public UserControllerData UserControllerData { set => userControllerData = value; }

    private void Awake()
    {
        Debug.Assert(animator != null);

    }

    public override void Init(Unit unit)
    {
        CharacterInfo characterInfo = unit as CharacterInfo;
        Assert.IsNotNull(characterInfo);


    }
}
