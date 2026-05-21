using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public readonly struct LoadLabelsData
{
    public readonly IReadOnlyList<string> labels;
    public readonly Addressables.MergeMode mergeMode;

    public LoadLabelsData(IReadOnlyList<string> labels, Addressables.MergeMode mergeMode)
    {
        this.labels = labels;
        this.mergeMode = mergeMode;
    }
}

public class AddressableBundleLoader : Singleton<AddressableBundleLoader>
{
    private Dictionary<string, AsyncOperationHandle> operationHandles;

    public void InitInstance()
    {
        operationHandles = new Dictionary<string, AsyncOperationHandle>(256);
    }

    public bool IsCachedAsset(string assetName)
    {
        return operationHandles.ContainsKey(assetName);
    }

    public void LoadAssetAsync<T>(string assetName, Action<T> successAction, Action<string> failedAction = null) where T : UnityEngine.Object
    {
        if (operationHandles.ContainsKey(assetName))
        {
            successAction?.Invoke((T)operationHandles[assetName].Result);
        }
        else
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetName);
            bool added = operationHandles.TryAdd(assetName, default);
            handle.Completed += (AsyncOperationHandle<T> result) =>
            {
                switch (result.Status)
                {
                    case AsyncOperationStatus.Succeeded:
#if UNITY_EDITOR
                        Debug.Assert(added, $"Asset Name: {assetName}");
#endif
                        if (added)
                        {
                            operationHandles[assetName] = result;
                        }
                        successAction?.Invoke(handle.Result);
                        break;
                    case AsyncOperationStatus.Failed:
                        failedAction?.Invoke(assetName);
                        Addressables.Release(handle);
                        break;
                }
            };
        }
    }

    public async UniTask<int> GetResourceLocationCount(string labelName)
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(labelName);

        IList<IResourceLocation> locations = await locationsHandle.ToUniTask();
        Addressables.Release(locationsHandle);
        return locations.Count;
    }

    public async UniTask<int> GetResourceLocationCount(IReadOnlyList<string> labelNames, Addressables.MergeMode mergeMode)
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(labelNames, mergeMode);

        IList<IResourceLocation> locations = await locationsHandle.ToUniTask();
        Addressables.Release(locationsHandle);
        return locations.Count;
    }

    public async UniTask LoadSpritesAsync(string labelName, Action<Sprite> onAssetLoadEnd, Action onFinished = null)
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(labelName, typeof(Sprite));

        IList<IResourceLocation> locations = await locationsHandle.ToUniTask();

        var loadOps = new List<AsyncOperationHandle>(locationsHandle.Result.Count);

        foreach (IResourceLocation location in locations)
        {
            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(location);
            string assetName = location.PrimaryKey;
            bool added = operationHandles.TryAdd(assetName, default);
            handle.Completed += (AsyncOperationHandle<Sprite> result) =>
            {
#if UNITY_EDITOR
                Debug.Assert(added, $"Asset Name: {assetName}");
#endif
                if (added)
                {
                    operationHandles[assetName] = result;
                }
                onAssetLoadEnd?.Invoke(result.Result);
            };
            loadOps.Add(handle);
        }

        await Addressables.ResourceManager.CreateGenericGroupOperation(loadOps, false);

        onFinished?.Invoke();
    }

    public async UniTask LoadSpritesAsync(LoadLabelsData data, Action<Sprite> onAssetLoadEnd, Action onFinished = null)
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(data.labels, data.mergeMode);

        IList<IResourceLocation> locations = await locationsHandle.ToUniTask();

        var loadOps = new List<AsyncOperationHandle>(locationsHandle.Result.Count);

        foreach (IResourceLocation location in locations)
        {
            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(location);
            string assetName = location.PrimaryKey;
            bool added = operationHandles.TryAdd(assetName, default);
            handle.Completed += (AsyncOperationHandle<Sprite> result) =>
            {
#if UNITY_EDITOR
                Debug.Assert(added, $"Asset Name: {assetName}");
#endif
                if (added)
                {
                    operationHandles[assetName] = result;
                }
                onAssetLoadEnd?.Invoke(result.Result);
            };
            loadOps.Add(handle);
        }

        await Addressables.ResourceManager.CreateGenericGroupOperation(loadOps, true);

        onFinished?.Invoke();
    }


    public async UniTask LoadAssetsAsync<T>(string labelName, Action<T> onAssetLoadEnd, Action onFinished = null) where T : UnityEngine.Object
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(labelName, typeof(T));

        IList<IResourceLocation> locations = await locationsHandle.ToUniTask();

        var loadOps = new List<AsyncOperationHandle>(locationsHandle.Result.Count);

        foreach (IResourceLocation location in locations)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(location);
            string assetName = location.PrimaryKey;
            bool added = operationHandles.TryAdd(assetName, default);
            handle.Completed += (AsyncOperationHandle<T> result) =>
            {
#if UNITY_EDITOR
                Debug.Assert(added, $"Asset Name: {assetName}");
#endif
                if (added)
                {
                    operationHandles[assetName] = result;
                }
                onAssetLoadEnd?.Invoke(result.Result);
            };
            loadOps.Add(handle);
        }

        await Addressables.ResourceManager.CreateGenericGroupOperation(loadOps, false);

        onFinished?.Invoke();
    }

    public async UniTask LoadAssetsAsync<T>(LoadLabelsData data, Action<T> onAssetLoadEnd, Action onFinished = null) where T : UnityEngine.Object
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(data.labels, data.mergeMode, typeof(T));

        IList<IResourceLocation> locations = await locationsHandle.ToUniTask();

        var loadOps = new List<AsyncOperationHandle>(locationsHandle.Result.Count);

        foreach (IResourceLocation location in locations)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(location);
            string assetName = location.PrimaryKey;
            bool added = operationHandles.TryAdd(assetName, default);
            handle.Completed += (AsyncOperationHandle<T> result) =>
            {
#if UNITY_EDITOR
                Debug.Assert(added, $"Asset Name: {assetName}");
#endif
                if (added)
                {
                    operationHandles[assetName] = result;
                }
                onAssetLoadEnd?.Invoke(result.Result);
            };
            loadOps.Add(handle);
        }

        await Addressables.ResourceManager.CreateGenericGroupOperation(loadOps, true);

        onFinished?.Invoke();
    }

    public void LoadSceneAsync(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single, bool activeOnLoad = true, Action onFinished = null)
    {
        AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> handle = Addressables.LoadSceneAsync(sceneName, loadSceneMode, activeOnLoad);
        handle.Completed += (AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> obj) =>
        {
            onFinished?.Invoke();
        };
    }

    public void ReleaseScene(UnityEngine.ResourceManagement.ResourceProviders.SceneInstance scene, Action onFinished = null)
    {
        AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> handle = Addressables.UnloadSceneAsync(scene);
        handle.Completed += (AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> result) =>
        {
            onFinished?.Invoke();
        };
    }

    public T TryGetLoadedAsset<T>(string assetName) where T : UnityEngine.Object
    {
        if (operationHandles.TryGetValue(assetName, out AsyncOperationHandle handle))
        {
            return handle.Result as T;
        }

        return null;
    }

    /// <summary>
    /// 에셋 로드 밑 캐싱이 완료된 경우에만 동기 생성
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="parentTrans"></param>
    public GameObject Instantiate(string key, Transform parentTrans)
    {
        if (operationHandles.ContainsKey(key))
        {
            var handle = Addressables.InstantiateAsync(key, parentTrans);
            GameObject result = handle.WaitForCompletion();

            return result;
        }
        return null;
    }

    /// <summary>
    /// 에셋 로드 밑 캐싱이 완료된 경우에만 비동기 생성
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="parentTrans"></param>
    /// <param name="successAction"></param>
    /// <param name="failedAction"></param>
    public void InstantiateAsync(string key, Transform parentTrans, Action<string, GameObject> successAction, Action failedAction = null)
    {
        if (operationHandles.ContainsKey(key))
        {
            var handle = Addressables.InstantiateAsync(key, parentTrans);

            handle.Completed += (AsyncOperationHandle<GameObject> completed) =>
            {
                switch (completed.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        successAction?.Invoke(key, completed.Result);
                        break;
                    default:
                        Addressables.Release(handle);
                        failedAction?.Invoke();
                        break;
                }
            };
        }
    }

    public void DestroyInstantiateObj(GameObject obj)
    {
        Assert.IsNotNull(obj);

        Addressables.ReleaseInstance(obj);
    }

    public void ReleaseLoadedAsset(string assetName)
    {
        if (operationHandles.TryGetValue(assetName, out var handle))
        {
            Addressables.Release(handle);
            operationHandles.Remove(assetName);
        }
    }

    public async UniTask ReleaseLoadedAssetByLabel(string label)
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(label);

        IList<IResourceLocation> locations = await locationsHandle.ToUniTask();

        foreach (IResourceLocation location in locations)
        {
            string assetName = location.PrimaryKey;
            ReleaseLoadedAsset(assetName);
        }
        Addressables.Release(locationsHandle);
    }

    public async UniTask ReleaseLoadedAssetByLabel(LoadLabelsData data)
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(data.labels, data.mergeMode);

        IList<IResourceLocation> locations = await locationsHandle.ToUniTask();

        foreach (IResourceLocation location in locations)
        {
            string assetName = location.PrimaryKey;
            ReleaseLoadedAsset(assetName);
        }
        Addressables.Release(locationsHandle);
    }

    public void ReleaseAllAssets()
    {
        foreach (var obj in operationHandles.Values)
        {
            Addressables.Release(obj);
        }

        operationHandles.Clear();
    }

    public void ClearData()
    {
        ReleaseAllAssets();
    }
}
