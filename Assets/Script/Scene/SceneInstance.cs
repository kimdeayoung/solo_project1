using UnityEngine;

public abstract class SceneInstance : MonoBehaviour
{
    protected PreSceneLoadProcess loadProcess;

    protected PreLoadState[] preLoadConditions = new PreLoadState[(int)PreLoadCondition.Length];

    public abstract void PreLoad();

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

    public virtual void Release()
    {
        if (loadProcess != null)
        {
            loadProcess.ReleaseLoadedAssets();
        }
    }
}
