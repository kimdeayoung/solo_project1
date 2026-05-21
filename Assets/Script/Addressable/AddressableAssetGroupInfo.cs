using System.Collections;
using System.Collections.Generic;
using System.Xml;
#if UNITY_EDITOR
using UnityEditor.AddressableAssets.Settings;
#endif
using UnityEngine;

public class AddressableAssetGroupInfo
{
    /// <summary>
    /// string = GroupName, List<AddressAbleAssetInfo> = AddressAbleAssetInfos
    /// </summary>
    private Dictionary<string, List<AddressableAssetInfo>> assetInfos;

    private const string AssetInfosKey = "AddressAbleAssetGroupInfo";

    public AddressableAssetGroupInfo()
    {
        assetInfos = new Dictionary<string, List<AddressableAssetInfo>>();
    }

#if UNITY_EDITOR
    public void InitAssetGroupData_Simul()
    {
        assetInfos.Clear();

        List<AddressableAssetGroup> addressableGroups = AddressablePath.GetAddressableGroups();
        assetInfos.EnsureCapacity(addressableGroups.Count);
        for (int i = 0; i < addressableGroups.Count; ++i)
        {
            string groupName = addressableGroups[i].Name;

            List<AddressableAssetEntry> entries = new List<AddressableAssetEntry>(addressableGroups[i].entries);
            List<AddressableAssetInfo> assetInfos = new List<AddressableAssetInfo>(entries.Count);

            for (int j = 0; j < entries.Count; ++j)
            {
                AddressableAssetEntry assetEntry = entries[j];
                if(assetEntry.IsFolder)
                {
                    continue;
                }
                AddressableAssetInfo assetInfo = new AddressableAssetInfo(groupName, assetEntry);
                assetInfos.Add(assetInfo);
            }
            this.assetInfos.Add(groupName, assetInfos);
        }
    }
#endif

    public List<AddressableAssetInfo> GetAddressAbleAssetInfosOrNull(string groupName)
    {
        List<AddressableAssetInfo> assetInfos = null;
        this.assetInfos.TryGetValue(groupName, out assetInfos);
        return assetInfos;
    }

    public void Read(XmlReader xmlReader)
    {
        assetInfos.Clear();

        while (xmlReader.EOF == false)
        {
            xmlReader.ReadStartElement(AssetInfosKey);
            while (xmlReader.IsStartElement("Info"))
            {
                AddressableAssetInfo assetInfo = new AddressableAssetInfo();
                assetInfo.Read(xmlReader);

                if (assetInfos.ContainsKey(assetInfo.GroupName))
                {
                    assetInfos[assetInfo.GroupName].Add(assetInfo);
                }
                else
                {
                    List<AddressableAssetInfo> assetInfos = new List<AddressableAssetInfo>();
                    assetInfos.Add(assetInfo);
                    this.assetInfos.Add(assetInfo.GroupName, assetInfos);
                }
            }
            xmlReader.ReadEndElement();
        }
    }

    public void Write(XmlWriter xmlWriter)
    {
        xmlWriter.WriteStartElement(AssetInfosKey);
        foreach (string groupName in assetInfos.Keys)
        {
            List<AddressableAssetInfo> infos = assetInfos[groupName];
            for (int i = 0; i < assetInfos.Count; ++i)
            {
                infos[i].Write(xmlWriter);
            }
        }
        xmlWriter.WriteEndElement();
    }
}
