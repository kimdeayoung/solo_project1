using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Joystick joyStick;

    public Joystick JoyStick { get => joyStick; }

    private void Awake()
    {
        
    }
}
