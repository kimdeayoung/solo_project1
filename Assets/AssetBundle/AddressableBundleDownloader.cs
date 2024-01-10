using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableBundleDownloader : SingletonWithMono<AddressableBundleDownloader>
{
    private enum DownloaderState
    {
        None,
        NoDownloadRequired,
        DownloadRequired,
        Downloading,
        AllDownloadEnd
    }

    public class DownloadInfo
    {
        private string bundleName;
        private ulong downloadBytes;
        private ulong totalBytes;

        public DownloadInfo(string bundleName, ulong fileSize)
        {
            this.bundleName = bundleName;
            downloadBytes = 0u;
            totalBytes = fileSize;
        }

        public string BundleName { get => bundleName; }


        public float DownloadProgress
        {
            get
            {
                if (totalBytes == 0u)
                {
                    return 0f;
                }

                return (float)downloadBytes / totalBytes;
            }
        }

        public ulong DownloadBytes { get => downloadBytes; set => downloadBytes = value; }
        public ulong TotalBytes { get => totalBytes; }

        public bool IsDone()
        {
            return (DownloadProgress - 1.0f) >= float.Epsilon;
        }
    }

    private DownloaderState currentState;

    private int checkDownloadSizeCount;
    private Queue<DownloadInfo> downloadInfoQueue;
    private DownloadInfo currentDownloadInfo;

    private AsyncOperationHandle downloadDependencieHandle;
    private Action updateDownloadFunc;
    private Action downloadEndAction;
    private Action<ulong> initBundleDownloadInfosCallback;

    private ulong currentDownloadSize;
    private ulong totalDownloadSize;

    public ulong CurrentDownloadSize => currentDownloadSize;
    public ulong TotalDownloadSize => totalDownloadSize;


    public void InitInstance()
    {
        downloadInfoQueue = new Queue<DownloadInfo>();
    }

    public void InitBundleDownloadInfos(Action<ulong> callback)
    {
        checkDownloadSizeCount = 0;
        totalDownloadSize = 0;
        currentState = DownloaderState.None;
        initBundleDownloadInfosCallback = callback;
        List<string> bundleLabels = AddressablePath.BundleLabels;

        foreach (string bundleLabel in bundleLabels)
        {
            InitBundleDownloadInfo(bundleLabel);
        }       
    }

    private void InitBundleDownloadInfo(string bundleLabel)
    {
        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(bundleLabel);
        getDownloadSize.Completed += (AsyncOperationHandle<long> obj) =>
        {
            switch (obj.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    //Debug.Log($"{bundleLabel} : size {obj.Result}");
                    if (obj.Result > 0)
                    {
                        downloadInfoQueue.Enqueue(new DownloadInfo(bundleLabel, Convert.ToUInt64(obj.Result)));
                        totalDownloadSize += Convert.ToUInt64(obj.Result);
                    }
                    break;
                case AsyncOperationStatus.Failed:
                    Debug.Log("Get Size Failed");
                    break;
            }

            Addressables.Release(obj);
            if (AddressablePath.BundleLabels.Count <= ++checkDownloadSizeCount)
            {
                if (downloadInfoQueue.Count > 0)
                {
                    currentState = DownloaderState.DownloadRequired;
                }
                else
                {
                    currentState = DownloaderState.NoDownloadRequired;
                }
                initBundleDownloadInfosCallback?.Invoke(totalDownloadSize);
            }
        };
    }

    public void DownloadAssetBundlesAsync(Action downloadEndAction = null)
    {
        switch (currentState)
        {
            case DownloaderState.DownloadRequired:
                DownloadDependenciesAsync();
                updateDownloadFunc = () =>
                {
                    float value = Convert.ToSingle(currentDownloadInfo.TotalBytes) * downloadDependencieHandle.PercentComplete;
                    currentDownloadInfo.DownloadBytes = Convert.ToUInt64(value);
                    Debug.Log($"DownloadBytes : {currentDownloadInfo.DownloadBytes}");
                    Debug.Log($"Download Percent : {GetTotalDownloadRatio()} %");
                };
                this.downloadEndAction = downloadEndAction;
                break;
            default:
                downloadEndAction?.Invoke();
                Debug.Log("No Need Download!");
                break;
        }
    }

    private void DownloadDependenciesAsync()
    {
        currentDownloadInfo = downloadInfoQueue.Dequeue();
        downloadDependencieHandle = Addressables.DownloadDependenciesAsync(currentDownloadInfo.BundleName);
        downloadDependencieHandle.Completed += DownloadBundleCompleted;
        currentState = DownloaderState.Downloading;
    }

    private void Update()
    {
        updateDownloadFunc?.Invoke();
    }

    private void DownloadBundleCompleted(AsyncOperationHandle obj)
    {
        currentDownloadSize += currentDownloadInfo.DownloadBytes;

        if (downloadInfoQueue.Count > 0)
        {
            DownloadDependenciesAsync();
        }
        else
        {
            currentState = DownloaderState.AllDownloadEnd;

            currentDownloadInfo = null;
            updateDownloadFunc = null;

            downloadEndAction?.Invoke();
            downloadEndAction = null;
        }

        Addressables.Release(obj);
    }

    public void ClearAssetBundles()
    {
        List<string> bundleLabels = AddressablePath.BundleLabels;

        foreach (string bundleLabel in bundleLabels)
        {
            Addressables.ClearDependencyCacheAsync(bundleLabel);
        }
        Debug.Log("ClearAssetBundles()");
    }

    public float GetTotalDownloadRatio()
    {
        if (totalDownloadSize == 0)
        {
            return 1.0f;
        }
        else
        {
            float currentSize = (float)currentDownloadSize;
            if (currentDownloadInfo != null)
            {
                currentSize += (float)currentDownloadInfo.DownloadBytes;
            }
            return currentSize / totalDownloadSize;
        }
    }

    public ulong GetTotalDownloadSize()
    {
        ulong currentSize = currentDownloadSize;
        if (currentDownloadInfo != null)
        {
            currentSize += currentDownloadInfo.DownloadBytes;
        }
        return currentSize;
    }

    public void ClearData()
    {
        downloadInfoQueue.Clear();
        currentDownloadSize = 0;
        currentState = DownloaderState.None;
        totalDownloadSize = 0;
    }
}
