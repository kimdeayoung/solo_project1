using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PreSceneLoadProcess
{
    private int currentLoadCount;
    private int totalLoadCount;

    public float Percent { get; private set; }

    protected List<string> singleAssetNames;

    protected List<string> labelNames;
    protected List<LoadLabelsData> labelsDatas;

    private Action onLoadEndAction;

    protected PreSceneLoadProcess(Action onLoadEndAction)
    {
        this.onLoadEndAction = onLoadEndAction;
    }

    public async UniTaskVoid LoadAssets()
    {
        AddressableBundleLoader loader = AddressableBundleLoader.Instance;
        totalLoadCount = 0;

        if (singleAssetNames != null)
        {
            totalLoadCount += singleAssetNames.Count;
        }

        if (labelNames != null)
        {
            int labelCount = labelNames.Count;
            for (int i = 0; i < labelCount; i++)
            {
                totalLoadCount += await loader.GetResourceLocationCount(labelNames[i]);
            }
        }

        if (labelsDatas != null)
        {
            int labelCount = labelsDatas.Count;
            for (int i = 0; i < labelCount; i++)
            {
                totalLoadCount += await loader.GetResourceLocationCount(labelsDatas[i].labels, labelsDatas[i].mergeMode);
            }
        }
        
        if (totalLoadCount == 0)
        {
            return;
        }

        currentLoadCount = 0;

        if (singleAssetNames != null)
        {
            int singleAssetLoadCount = singleAssetNames.Count;
            for (int i = 0; i < singleAssetLoadCount; i++)
            {
                loader.LoadAssetAsync<UnityEngine.Object>(singleAssetNames[i], AssetLoadSuccess, OnSingleAssetLoadFailed);
            }
        }

        if (labelNames != null)
        {
            int labelCount = labelNames.Count;
            for (int i = 0; i < labelCount; i++)
            {
                loader.LoadAssetsAsync(labelNames[i], AssetLoadSuccess).Forget();
            }
        }

        if (labelsDatas != null)
        {
            int labelCount = labelsDatas.Count;
            for (int i = 0; i < labelCount; i++)
            {
                loader.LoadAssetsAsync(labelsDatas[i], AssetLoadSuccess).Forget();
            }
        }
    }

    public void ReleaseLoadedAssets()
    {
        AddressableBundleLoader loader = AddressableBundleLoader.Instance;
        if (singleAssetNames != null)
        {
            int singleAssetLoadCount = singleAssetNames.Count;
            for (int i = 0; i < singleAssetLoadCount; i++)
            {
                loader.ReleaseLoadedAsset(singleAssetNames[i]);
            }
        }

        if (labelNames != null)
        {
            int labelCount = labelNames.Count;
            for (int i = 0; i < labelCount; i++)
            {
                loader.ReleaseLoadedAssetByLabel(labelNames[i]).Forget();
            }
        }

        if (labelsDatas != null)
        {
            int labelCount = labelsDatas.Count;
            for (int i = 0; i < labelCount; i++)
            {
                loader.ReleaseLoadedAssetByLabel(labelsDatas[i]).Forget();
            }
        }
    }

    #region AssetLoadEnd
    private void AssetLoadSuccess(UnityEngine.Object obj)
    {
#if UNITY_EDITOR
        Debug.Log($"Load Success: {obj.name}");
#endif
        OnLoadProcessEnd();
    }

    private void AssetLoadSuccess(string name)
    {
#if UNITY_EDITOR
        Debug.Log($"Load Success: {name}");
#endif
        OnLoadProcessEnd();
    }

    private void OnSingleAssetLoadFailed(string name)
    {
        Debug.LogError($"Load Failed: {name}");
        OnLoadProcessEnd();
    }
    #endregion

    private void OnLoadProcessEnd()
    {
        ++currentLoadCount;
        Percent = (float)currentLoadCount / totalLoadCount;

        if (Percent >= 1.0f)
        {
            Percent = 1.0f;
            onLoadEndAction?.Invoke();
        }
    }
}
