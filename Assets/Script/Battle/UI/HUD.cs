using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : UIBase
{
    [SerializeField]
    private Joystick joyStick;

    [SerializeField]
    private ActionBtn[] actionBtns;

    private Player player;

    public void Init()
    {
        joyStick.OnUpdateDirection += UpdatePlayerMoveDirection;
    }

    public void RegisterPlayer(Player player)
    {
        this.player = player;

        //IReadOnlyList<BaseActionData> baseActions = player.ActionDatas;
    }

    private void UpdatePlayerMoveDirection(Vector2 direction, float moveIntensity)
    {
        player.SetMoveDirection(new Vector3(direction.x, 0f, direction.y), moveIntensity);
    }
}
