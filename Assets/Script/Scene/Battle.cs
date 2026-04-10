using System.Collections.Generic;
using UnityEngine;

public class Battle : SceneInstance
{
    public BattleUIController BattleUIController {  get; private set; }

    private List<BattleUnit> battleUnits = new List<BattleUnit>();

    public override void PreLoad()
    {
        loadProcess = new BattleLoadProcess(OnStart);
        loadProcess.LoadAssets();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime * TimeManager.Instance.GameSpeed;

        int loopCount = battleUnits.Count;
        for (int i = 0; i < loopCount; i++)
        {
            battleUnits[i].OnUpdate(deltaTime);
        }
    }

    private void FixedUpdate()
    {
        float fixedDeltaTime = Time.fixedDeltaTime;

        int loopCount = battleUnits.Count;
        for (int i = 0; i < loopCount; i++)
        {
            battleUnits[i].OnFixedUpdate(fixedDeltaTime);
        }
    }

    public void RegisterBattleUnit(BattleUnit unit)
    {
        battleUnits.Add(unit);
    }

    public void UnregisterBattleUnit(BattleUnit unit)
    {
        battleUnits.Remove(unit);
    }
}
