using PlayerState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : BattleUnit
{
    [SerializeField] private PlayerStatusStat _stat;
    [SerializeField] private Animator _animator;

    private UserControllerData userControllerData;

    private Joystick Joystick { get => userControllerData.JoyStick; }
    private ActionBtn[] ActionBtns { get => userControllerData.ActionBtns; }

    public UserControllerData UserControllerData { set => userControllerData = value; }

    private List<BaseActionData> _actionDatas;

    private void Awake()
    {
        Debug.Assert(_animator != null);
    }

    public override void Init()
    {
        base.Init();

        BehaviourController.AddBehaviourState<IdleState>(this)
                           .AddBehaviourState<StunState>(this);
        BehaviourController.SetBehaviourState(UnitState.Idle);

        {
            BaseActionDataSO[] actionDatas = _stat.ActionDatas;
            int actionDataCount = actionDatas.Length;
            _actionDatas = new List<BaseActionData>(actionDataCount);
            for (int i = 0; i < actionDataCount; i++)
            {

            }
        }
        

        Status = new PlayaerStatus(this, _stat);
    }

    public override void OnUpdate(float deltaTime)
    {
        BehaviourController.OnUpdate(deltaTime);
    }

    public override void UpdateActionDatas(float deltaTime)
    {
        int loopCount = _actionDatas.Count;
        for (int i = 0; i < loopCount; i++)
        {
            _actionDatas[i].OnUpdate(deltaTime, Status.Haste);
        }
    }
}
