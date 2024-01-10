using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControllerData
{
    private Joystick joyStick;

    public UserControllerData(Joystick joyStick)
    {
        this.joyStick = joyStick;
    }

    public Joystick JoyStick { get => joyStick; }


}
