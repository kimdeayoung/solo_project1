using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BattleCharacter : BattleUnit
{
    [SerializeField] private PlayerStatusStat _stat;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _rotateSpeed;

    private UserControllerData userControllerData;

    private Joystick Joystick { get => userControllerData.JoyStick; }
    private ActionBtn[] ActionBtns { get => userControllerData.ActionBtns; }

    public UserControllerData UserControllerData { set => userControllerData = value; }

    private void Awake()
    {
        Debug.Assert(_animator != null);
    }

    public override void Init()
    {
        base.Init();

        status = new PlayaerStatus(this, _stat);
    }

    public void Update()
    {
        UnitState playerState = GetUnitState();

        switch (playerState)
        {
            case UnitState.Idle:
                PlayerMove();
                break;
        }
    }

    private void PlayerMove()
    {
        if (Joystick.IsTouchJoyStick)
        {
            Vector3 moveDir = Joystick.LeverPos.normalized;
            transform.Translate(moveDir * status.MoveSpeed);
        }
    }
}
