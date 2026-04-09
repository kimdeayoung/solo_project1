using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : UpdateableManager<TimeManager>
{
    public float GameSpeed { get; private set; }

    public override void Init()
    {
        base.Init();

        GameSpeed = 1.0f;
    }

    public override void OnUpdate()
    {
    }

    public override void ClearData()
    {
        GameSpeed = 1.0f;
    }
}
