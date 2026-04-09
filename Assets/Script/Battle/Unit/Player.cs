using PlayerState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : BattleUnit
{
    [SerializeField] private PlayerStatusStat _stat;
    [SerializeField] private Animator _animator;

    [SerializeField] private Rigidbody _rigidBody;

    public UserControllerData UserControllerData { get; private set; }

    private List<BaseActionData> _actionDatas;

#if UNITY_EDITOR
    private void Awake()
    {
        Debug.Assert(_animator != null);
        Debug.Assert(_rigidBody != null);
    }
#endif

    public override void Init()
    {
        base.Init();

        //UserControllerData = new UserControllerData(battleUIController.Joystick, battleUIController.ActionBtns);

        BehaviourController.AddBehaviourState<IdleState>(this)
                           .AddBehaviourState<StunState>(this);
        BehaviourController.SetBehaviourState(UnitState.Idle);

        {
            BaseActionDataSO[] actionDatas = _stat.ActionDatas;
            int actionDataCount = actionDatas.Length;
            _actionDatas = new List<BaseActionData>(actionDataCount);
            for (int i = 0; i < actionDataCount; i++)
            {
                BaseActionDataSO actionDataSO = actionDatas[i];
                BaseActionData actionData = ActionDataPool.GetActionParameter(actionDataSO.ActionType);

                actionData.Init(this);
                actionData.ResetVariables(actionDataSO);
                _actionDatas.Add(actionData);
            }
        }
        

        Status = new PlayaerStatus(this, _stat);
    }

    public override void OnUpdate(float deltaTime)
    {
        BehaviourController.OnUpdate(deltaTime);
    }

    public override void OnFixedUpdate(float fixedDeltaTime)
    {
        BehaviourController.OnFixedUpdate(fixedDeltaTime);
    }

    public override void UpdateActionDatas(float deltaTime)
    {
        int loopCount = _actionDatas.Count;
        for (int i = 0; i < loopCount; i++)
        {
            _actionDatas[i].OnUpdate(deltaTime, Status.Haste);
        }
    }

    public void TranslateWithRotation(Vector3 targetDir, float fixedDeltaTime)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);
        Quaternion newRotation = Quaternion.RotateTowards(_rigidBody.rotation, targetRotation, _stat.Rotate * fixedDeltaTime);

        _rigidBody.MoveRotation(newRotation);

        Vector3 forward = _rigidBody.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 move = forward * _stat.MoveSpeed * fixedDeltaTime;
        _rigidBody.MovePosition(_rigidBody.position + move);
    }
}
