using UnityEngine;

public abstract class SceneInstance : MonoBehaviour
{
    public Camera MainCam { get; private set; }

    public UIController UIController { get; private set; }
    public SafeAreaController SafeAreaController { get; private set; }


    protected PreSceneLoadProcess loadProcess;

    protected PreLoadState[] preLoadConditions = new PreLoadState[(int)PreLoadCondition.Length];

    public virtual void PreLoad()
    {
        MainCam = Camera.main;

        UIController = GameObject.Find("UI").GetComponent<UIController>();
        UIController.Init();

        SafeAreaController = new SafeAreaController();
    }

    protected virtual void OnStart()
    {
    }

    public void SetPreLoadState(PreLoadCondition condition)
    {
        preLoadConditions[(int)condition] = PreLoadState.Ready;

        int loopCount = (int)PreLoadCondition.Length;
        for (int i = 0; i < loopCount; i++)
        {
            if (preLoadConditions[i] == PreLoadState.NotReady)
            {
                return;
            }
        }

        OnStart();
    }

    protected virtual void Update()
    {
        SafeAreaController.OnUpdate();
    }

    protected virtual void FixedUpdate()
    {
    }

    public virtual void Release()
    {
        UIController.Release();

        if (loadProcess != null)
        {
            loadProcess.ReleaseLoadedAssets();
        }
    }
}
