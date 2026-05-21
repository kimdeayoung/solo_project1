using System.Collections;
using System.Collections.Generic;
using System.Xml;
#if UNITY_EDITOR
using UnityEditor.AddressableAssets.Settings;
#endif
using UnityEngine;

public class AddressableAssetInfo
{
    private string groupName;
    private string assetName;

    private const string ElementKey = "Info";
    private const string GroupNameKey = "GroupName";
    private const string AssetNameKey = "AssetName";

    public string GroupName { get => groupName; }
    public string AssetNames { get => assetName; }

    public AddressableAssetInfo()
    {
    }

#if UNITY_EDITOR
    public AddressableAssetInfo(string groupName, AddressableAssetEntry assetEntry)
    {
        this.groupName = groupName;
        assetName = assetEntry.address;
    }
#endif

    public void Read(XmlReader xmlReader)
    {
        xmlReader.ReadStartElement(ElementKey);
        XmlHelper.ReadElement(xmlReader, GroupNameKey, ref groupName);
        XmlHelper.ReadElement(xmlReader, AssetNameKey, ref assetName);
        xmlReader.ReadEndElement();
    }

    public void Write(XmlWriter xmlWriter)
    {
        xmlWriter.WriteStartElement(ElementKey);
        //XmlHelper.WriteElement(xmlWriter, GroupNameKey, _groupName);
        //XmlHelper.WriteElement(xmlWriter, AssetNameKey, _assetName);
        xmlWriter.WriteEndElement();
    }
}
