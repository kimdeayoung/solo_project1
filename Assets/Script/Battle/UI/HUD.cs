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

        IReadOnlyList<BaseActionData> baseActions = player.ActionDatas;
        int actionCount = baseActions.Count;

        int loopCount = actionBtns.Length;
        for (int i = 0; i < loopCount; i++)
        {
            if (actionCount > i)
            {
                actionBtns[i].Init(player, baseActions[i]);
                actionBtns[i].gameObject.SetActive(true);
            }
            else
            {
                actionBtns[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdatePlayerMoveDirection(Vector2 direction, float moveIntensity)
    {
        player.SetMoveDirection(new Vector3(direction.x, 0f, direction.y), moveIntensity);
    }
}
