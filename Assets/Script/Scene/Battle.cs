using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class Battle : SceneInstance
{
    public Entitys Entity { get; private set; }
    public Transform EntityRoot { get; private set; }

    public BattleUIController BattleUIController { get; private set; }

    public override void PreLoad()
    {
        TimeManager.Instance.SetGameSpeed(0.0f);

        EntityRoot = transform.Find("BattleObjects");
        Entity = new Entitys();

        loadProcess = new BattleLoadProcess(OnAssetLoadEnd);
        loadProcess.LoadAssets().Forget();
    }

    private void OnAssetLoadEnd()
    {
        SetPreLoadState(PreLoadCondition.LoadAsset);

        Entity.Initialize().Forget();
    }

    protected override void OnStart()
    {
        TimeManager.Instance.SetGameSpeed(1.0f);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime * TimeManager.Instance.GameSpeed;

        Entity.OnUpdate(deltaTime);
    }

    private void FixedUpdate()
    {
        float fixedDeltaTime = Time.fixedDeltaTime;

        Entity.OnFixedUpdate(fixedDeltaTime);
    }
}
