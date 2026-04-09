using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct UserControllerData
{
    private readonly Joystick joyStick;
    private readonly ActionBtn[] actionBtns;

    public UserControllerData(Joystick joyStick, ActionBtn[] actionBtns)
    {
        this.joyStick = joyStick;
        this.actionBtns = actionBtns;
    }

    public Joystick JoyStick { get => joyStick; }
    public ActionBtn[] ActionBtns { get => actionBtns; }
}
