using UnityEngine;

namespace PlayerState
{
    public class IdleState : BehaviourState
    {
        private Player _player;
        private UserControllerData _userControllerData;

        public override UnitState UnitState => UnitState.Idle;

        public override void Init(BattleUnit owner)
        {
            base.Init(owner);

            _player = owner as Player;
            Debug.Assert(_player != null);
            _userControllerData = _player.UserControllerData;
        }

        public override void OnStart()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
            ownerBase.UpdateActionDatas(deltaTime);
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            UpdataeMove(fixedDeltaTime);
        }

        public override void OnEnd()
        {

        }

        private void UpdataeMove(float fixedDeltaTime)
        {
            Joystick joystick = _userControllerData.JoyStick;
            if (joystick.IsTouchJoyStick)
            {
                Vector2 joystickDir = joystick.LeverPos.normalized;
                _player.TranslateWithRotation(new Vector3(joystickDir.x, 0f, joystickDir.y), fixedDeltaTime);
            }
        }
    }
}

