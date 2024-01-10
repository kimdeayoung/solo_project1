using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public class AddressableBundleLoader : Singleton<AddressableBundleLoader>
{
    private Dictionary<string, AsyncOperationHandle<UnityEngine.Object>> operationHandles;

    private AtlasManagement atlasManagement;

    public void InitInstance()
    {
        operationHandles = new Dictionary<string, AsyncOperationHandle<UnityEngine.Object>>(256);

        atlasManagement = new AtlasManagement();
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
            AsyncOperationHandle<UnityEngine.Object> handle = Addressables.LoadAssetAsync<UnityEngine.Object>(assetName);
            handle.Completed += (AsyncOperationHandle<UnityEngine.Object> result) =>
            {
                switch (handle.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        operationHandles.Add(assetName, result);
                        successAction?.Invoke((T)handle.Result);
                        break;
                    case AsyncOperationStatus.Failed:
                        failedAction?.Invoke();
                        Addressables.Release(handle);
                        break;
                }
            };
        }
    }

    public T LoadAsset<T>(string assetName, Action failedAction = null) where T : UnityEngine.Object
    {
        if (operationHandles.ContainsKey(assetName))
        {
            return (T)operationHandles[assetName].Result;
        }
        else
        {
            AsyncOperationHandle<UnityEngine.Object> handle = Addressables.LoadAssetAsync<UnityEngine.Object>(assetName);
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    operationHandles.Add(assetName, handle);
                    return (T)handle.Result;
                default:
                    failedAction?.Invoke();
                    Addressables.Release(handle);
                    return null;
            }
        }
    }

    public void LoadAssetsAsync(string labelName, Action onFinished = null)
    {
        Assert.IsTrue(AddressablePath.BundleLabels.Contains(labelName), $"Check addressableLabel is Exist addressableLabel : {labelName}");

        //var handle = Addressables.LoadAssetsAsync(labelName, (UnityEngine.Object obj) =>
        //{
        //    if (_bundleCacheObjs.TryAdd(obj.name, obj) == false)
        //    {
        //        _duplicateBundleNames.Add(obj.name);
        //    }
        //});
        //
        //_bundleAssetsHandles.Add(labelName, handle);
        //
        //handle.Completed += (AsyncOperationHandle<IList<UnityEngine.Object>> obj) =>
        //{
        //    onFinished?.Invoke();
        //};
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
    /// 에셋 로드 밑 캐싱이 완료된 경우에만 null이 아닌 instantiateObj 리턴
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="parentTrans"></param>
    /// <returns></returns>
    public GameObject InstantiateOrNull(string key, Transform parentTrans)
    {
        if (operationHandles.ContainsKey(key))
        {
            var operation = Addressables.InstantiateAsync(key, parentTrans);
            operation.WaitForCompletion();

            switch (operation.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    return operation.Result;
                default:
                    return null;
            }
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

    public void LoadSpriteAtlas(string atlasName)
    {
        atlasManagement.LoadSpriteAtlas(atlasName);
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

    public void SetRawSprite(RawImage image, string rawSpriteName)
    {
        Assert.IsFalse(string.IsNullOrEmpty(rawSpriteName), "RawSpriteName is Null or Empty");
        Texture texture = LoadAsset<Texture>(rawSpriteName);
        Assert.IsNotNull(texture, $"RawSprite Can't Find RawSpriteName Name : {rawSpriteName}");
        image.texture = texture;
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

    public void ReleaseLoadedAssets(string addressableLabel)
    {
        //AsyncOperationHandle<IList<UnityEngine.Object>> handle;
        //if (_bundleAssetsHandles.Remove(addressableLabel, out handle))
        //{
        //    foreach (var obj in handle.Result)
        //    {
        //        string objName = obj.name;
        //        if (_duplicateBundleNames.Contains(objName) == false)
        //        {
        //            _bundleCacheObjs.Remove(objName);
        //        }
        //    }
        //
        //    Addressables.Release(handle);
        //}
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
