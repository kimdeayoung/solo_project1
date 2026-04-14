using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Battle : SceneInstance
{
    public Entitys Entity { get; private set; }
    public Transform EntityRoot { get; private set; }

    public BattleUIController BattleUIController { get; private set; }

    public override void PreLoad()
    {
        base.PreLoad();

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

        {
            UIController.ShowInstant<HUD>();
        }

        {
            BattleUnit player = Entity.FindBattleUnitOrNull(BattleUnitType.Player);
            Debug.Assert(player != null);

            GlobalVariables globalVariables = GameManager.Instance.GlobalVariables;

            ObjectFollower objectFollower = MainCam.transform.AddComponent<ObjectFollower>();
            objectFollower.ResetVariables(player.gameObject, globalVariables.FollowCamOffset, globalVariables.FollowCamLookAtTarget);
            //objectFollower.SetTarget(player.gameObject);
        }
    }

    protected override void Update()
    {
        base.Update();
        float deltaTime = Time.deltaTime * TimeManager.Instance.GameSpeed;

        Entity.OnUpdate(deltaTime);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        float fixedDeltaTime = Time.fixedDeltaTime;

        Entity.OnFixedUpdate(fixedDeltaTime);
    }
}
