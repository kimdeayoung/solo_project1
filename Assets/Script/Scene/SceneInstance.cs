using UnityEngine;

public abstract class SceneInstance : MonoBehaviour
{
    protected PreSceneLoadProcess loadProcess;

    public abstract void PreLoad();

    protected virtual void OnStart()
    {
    }

    public virtual void Release()
    {
        if (loadProcess != null)
        {
            loadProcess.ReleaseLoadedAssets();
        }
    }
}
