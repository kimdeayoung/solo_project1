using UnityEngine;

namespace PlayerState
{
    public class IdleState : BehaviourState
    {
        private Player player;
        private UserControllerData userControllerData;

        public override UnitState UnitState => UnitState.Idle;

        public override void Init(BattleUnit owner)
        {
            base.Init(owner);

            player = owner as Player;
            Debug.Assert(player != null);
            userControllerData = player.UserControllerData;
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
            UpdateMove(fixedDeltaTime);
        }

        public override void OnEnd()
        {

        }

        private void UpdateMove(float fixedDeltaTime)
        {
            Joystick joystick = userControllerData.JoyStick;
            if (joystick.IsTouchJoyStick)
            {
                Vector2 joystickDir = joystick.LeverPos.normalized;
                player.TranslateWithRotation(new Vector3(joystickDir.x, 0f, joystickDir.y), fixedDeltaTime);
            }
        }
    }
}

