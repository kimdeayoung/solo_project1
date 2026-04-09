using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UserControllerData
{
    private Joystick joyStick;
    private ActionBtn[] actionBtns;

    public UserControllerData(Joystick joyStick, ActionBtn[] actionBtns)
    {
        this.joyStick = joyStick;
        this.actionBtns = actionBtns;
    }

    public Joystick JoyStick { get => joyStick; }
    public ActionBtn[] ActionBtns { get => actionBtns; }
}
