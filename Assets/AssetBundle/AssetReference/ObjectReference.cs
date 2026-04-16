using AutoGroupGenerator;
using System;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class ObjectReference<T> : AssetReferenceT<T>, ISerializationCallbackReceiver where T : UnityEngine.Object
{
    [SerializeField] private string assetName;
    public string AssetName => assetName;

    public ObjectReference(string guid) : base(guid)
    {
    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(AssetGUID))
        {
            assetName = string.Empty;
            return;
        }

        AddressableAssetEntry entry = AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(AssetGUID, true);
        if (entry != null)
        {
            assetName = entry.address;
        }
#endif
    }

    public void OnAfterDeserialize()
    {

    }
}
