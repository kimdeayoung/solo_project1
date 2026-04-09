using Cysharp.Threading.Tasks;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public class AddressableBundleLoader : Singleton<AddressableBundleLoader>
{
    private Dictionary<string, AsyncOperationHandle> operationHandles;

    private AtlasManagement atlasManagement;

    public void InitInstance()
    {
        operationHandles = new Dictionary<string, AsyncOperationHandle>(256);

        atlasManagement = new AtlasManagement();

        LoadAssetsAsync("IngamePrefab").Forget();
    }

    private void Test(GameObject obj)
    {
    }
    private void Test2(GameObject obj)
    {

    }

    public bool IsCachedAsset(string assetName)
    {
        return operationHandles.ContainsKey(assetName);
    }

    public void LoadAssetAsync<T>(string assetName, Action<T> successAction, Action failedAction = null) where T : UnityEngine.Object
    {
        if (operationHandles.ContainsKey(assetName))
        {
            successAction?.Invoke((T)operationHandles[assetName].Result);
        }
        else
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetName);
            handle.Completed += (AsyncOperationHandle<T> result) =>
            {
                switch (handle.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        Debug.Assert(!operationHandles.ContainsKey(assetName));
#if UNITY_EDITOR
                        bool added = operationHandles.TryAdd(assetName, result);
                        Debug.Assert(added);
#else
                        operationHandles.TryAdd(assetName, result);
#endif
                        successAction?.Invoke(handle.Result);
                        break;
                    case AsyncOperationStatus.Failed:
                        failedAction?.Invoke();
                        Addressables.Release(handle);
                        break;
                }

                
            };
        }
    }

    public async UniTaskVoid LoadAssetsAsync(string labelName, Action onFinished = null)
    {
        //Assert.IsTrue(AddressablePath.BundleLabels.Contains(labelName), $"Check addressableLabel is Exist addressableLabel : {labelName}");
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(labelName);

        IList<IResourceLocation> locations = await locationsHandle.ToUniTask();

        var loadOps = new List<AsyncOperationHandle>(locationsHandle.Result.Count);

        foreach (IResourceLocation location in locations)
        {
            AsyncOperationHandle<UnityEngine.Object> handle = Addressables.LoadAssetAsync<UnityEngine.Object>(location);
            handle.Completed += (AsyncOperationHandle<UnityEngine.Object> result) =>
            {
                string assetName = location.PrimaryKey;
                Debug.Assert(!operationHandles.ContainsKey(assetName));
#if UNITY_EDITOR
                bool added = operationHandles.TryAdd(assetName, result);
                Debug.Assert(added);
#else
                operationHandles.TryAdd(assetName, result);
#endif
            };
            loadOps.Add(handle);
        }

        await Addressables.ResourceManager.CreateGenericGroupOperation(loadOps, true);

        onFinished?.Invoke();
    }

    public async UniTaskVoid LoadAssetsAsync(IReadOnlyList<string> labelNames, Addressables.MergeMode mergeMode, Action onFinished = null)
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(labelNames, mergeMode);

        IList<IResourceLocation> locations = await locationsHandle.ToUniTask();

        var loadOps = new List<AsyncOperationHandle>(locationsHandle.Result.Count);

        foreach (IResourceLocation location in locations)
        {
            AsyncOperationHandle<UnityEngine.Object> handle = Addressables.LoadAssetAsync<UnityEngine.Object>(location);
            handle.Completed += (AsyncOperationHandle<UnityEngine.Object> result) =>
            {
                string assetName = location.PrimaryKey;
                Debug.Assert(!operationHandles.ContainsKey(assetName));
#if UNITY_EDITOR
                bool added = operationHandles.TryAdd(assetName, result);
                Debug.Assert(added);
#else
                operationHandles.TryAdd(assetName, result);
#endif
                Addressables.Release(handle);
            };
            loadOps.Add(handle);
        }

        await Addressables.ResourceManager.CreateGenericGroupOperation(loadOps, true);

        onFinished?.Invoke();
    }

    public void LoadSceneAsync(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single, bool activeOnLoad = true, Action onFinished = null)
    {
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneName, loadSceneMode, activeOnLoad);
        handle.Completed += (AsyncOperationHandle<SceneInstance> obj) =>
        {
            onFinished?.Invoke();
        };
    }

    public void ReleaseScene(SceneInstance scene, Action onFinished = null)
    {
        AsyncOperationHandle<SceneInstance> handle = Addressables.UnloadSceneAsync(scene);
        handle.Completed += (AsyncOperationHandle<SceneInstance> result) =>
        {
            onFinished?.Invoke();
        };
    }

    /// <summary>
    /// 에셋 로드 밑 캐싱이 완료된 경우에만 비동기 생성
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="parentTrans"></param>
    /// <param name="successAction"></param>
    /// <param name="failedAction"></param>
    public void InstantiateAsync(string key, Transform parentTrans, Action<GameObject> successAction, Action failedAction = null)
    {
        if (operationHandles.ContainsKey(key))
        {
            var handle = Addressables.InstantiateAsync(key, parentTrans);

            handle.Completed += (AsyncOperationHandle<GameObject> completed) =>
            {
                switch (completed.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        successAction?.Invoke(completed.Result);
                        break;
                    default:
                        failedAction?.Invoke();
                        break;
                }
            };
        }
    }

    public void LoadSpriteAtlasAsync(string atlasName, Action onFinished = null)
    {
        atlasManagement.LoadSpriteAtlasAsync(atlasName, onFinished);
    }

    public void SetAtlasSprite(Image image, string spriteName)
    {
        Assert.IsFalse(string.IsNullOrEmpty(spriteName), "spriteName is Null or Empty");

        Sprite sprite = atlasManagement.GetSpriteOrNull(spriteName);

        Assert.IsNotNull(sprite, $"Sprite Can't Find Check Atlas Or SpriteName Name : {spriteName}");
        image.sprite = sprite;
    }

    public Sprite GetAtlasSprite(string spriteName)
    {
        Sprite sprite = atlasManagement.GetSpriteOrNull(spriteName);
        return sprite;
    }

    public void SetRawSpriteAsync(RawImage image, string rawSpriteName, Action onSuccess, Action onFailed)
    {
        Assert.IsFalse(string.IsNullOrEmpty(rawSpriteName), "RawSpriteName is Null or Empty");
        LoadAssetAsync(rawSpriteName, (Texture texture) =>
        {
            if (image != null)
            {
                image.texture = texture;
            }
            onSuccess?.Invoke();
        }, onFailed);
    }

    public List<string> GetSpriteNamesOrNull(string atlasName)
    {
        return atlasManagement.GetSpriteNamesOrNull(atlasName);
    }

    public void DestroyInstantiateObj(GameObject obj)
    {
        Assert.IsNotNull(obj);

        Addressables.ReleaseInstance(obj);
    }

    public void ReleaseLoadedAsset(string assetName)
    {
        if (operationHandles.ContainsKey(assetName))
        {
            var handle = operationHandles[assetName];

            Addressables.Release(handle);
            operationHandles.Remove(assetName);
        }
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
        atlasManagement.ClearData();
    }
}
