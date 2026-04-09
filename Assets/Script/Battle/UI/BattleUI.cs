using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Joystick joyStick;

    [SerializeField]
    private ActionBtn[] actionBtns;

    public Joystick JoyStick { get => joyStick; }
    public ActionBtn[] ActionBtns { get => actionBtns; }

    private void Awake()
    {
        
    }
}
