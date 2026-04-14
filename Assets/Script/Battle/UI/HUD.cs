using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : UIBase
{
    [SerializeField]
    private Joystick joyStick;

    [SerializeField]
    private ActionBtn[] actionBtns;

    public Joystick JoyStick { get => joyStick; }
    public ActionBtn[] ActionBtns { get => actionBtns; }
}
