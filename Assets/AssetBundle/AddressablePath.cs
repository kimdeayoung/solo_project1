using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif
using UnityEngine;
using UnityEngine.Assertions;

public static class AddressablePath
{
    private const string AddressableLabelsInfoKey = "AddressableLabelsInfo";
    private const string BundleLabelKey = "BundleLabel";

    private const string groupName_Atlas = "Atlases";
    private const string groupName_Prefab = "Prefab";

    private static List<string> bundleLabels = new List<string>();
    private static AddressableAssetGroupInfo assetGroupInfo = new AddressableAssetGroupInfo();

    public static List<string> BundleLabels
    {
        get
        {
            bundleLabels = new List<string>
                {
                    //"default",
                    "Prefab",
                    //"Sound", 
                    "Tables",
                    //"Effects",
                    //"SkillEffectData",
                    "Atlases",
                    "Spine",
                    "Sprites",
                    "RawSprites",
                };

            return bundleLabels;
        }
    }

    public static AddressableAssetGroupInfo AssetGroupInfo { get => assetGroupInfo; }

    public static string GroupName_Atlas => groupName_Atlas;
    public static string GroupName_Prefab => groupName_Prefab;

    private static string GetAddressAbleBundleLabelsInfoXmlPath()
    {
        return Path.Combine(Application.persistentDataPath, "addressAbleBundleLabelsInfo.xml");
    }

    private static string GetAddressAbleAssetGroupInfoXmlPath()
    {
        return Path.Combine(Application.persistentDataPath, "addressAbleAssetGroupInfo.xml");
    }

    public static void ReadAddressAbleBundleLabelsInfoXmlFile()
    {
        string filePath = GetAddressAbleBundleLabelsInfoXmlPath();

        bundleLabels.Clear();
        using (XmlReader xmlReader = XmlReader.Create(filePath))
        {
            xmlReader.ReadStartElement(AddressableLabelsInfoKey);
            while (xmlReader.IsStartElement(BundleLabelKey))
            {
                string bundleLabel = string.Empty;
                //XmlHelper.ReadXML(xmlReader, BundleLabelKey, ref bundleLabel);
                bundleLabels.Add(bundleLabel);
            }
            xmlReader.ReadEndElement();
        }
    }

    public static void ReadAddressAbleAssetGroupInfoXmlFile()
    {
        string filePath = GetAddressAbleAssetGroupInfoXmlPath();

        using (XmlReader xmlReader = XmlReader.Create(filePath))
        {
            assetGroupInfo.Read(xmlReader);
        }
    }

    public static List<string> GetAddressableAssetNames(string groupName)
    {
        List<AddressableAssetInfo> assetInfos = assetGroupInfo.GetAddressAbleAssetInfosOrNull(groupName);
        Assert.IsNotNull(assetInfos, $"Can't find Group, Check Addressable Group Name : {groupName}");

        List<string> assetNames = new List<string>();
        for(int i = 0; i < assetInfos.Count; ++i)
        {
            assetNames.Add(assetInfos[i].AssetNames);
        }
        
        return assetNames;
    }

#if UNITY_EDITOR
    public static List<AddressableAssetGroup> GetAddressableGroups()
    {
        List<AddressableAssetGroup> addressableGroups = new List<AddressableAssetGroup>(AddressableAssetSettingsDefaultObject.Settings.groups);
        for (int i = 0; i < addressableGroups.Count;)
        {
            AddressableAssetGroup assetGroup = addressableGroups[i];
            if(assetGroup.Default || assetGroup.Name == "Built In Data")
            {
                addressableGroups.Remove(assetGroup);
            }
            else
            {
                ++i;
            }
        }
        return addressableGroups;
    }

    public static AddressableAssetGroup GetAddressableAssetGroupOrNull(string groupName)
    {
        List<AddressableAssetGroup> addressableGroups = GetAddressableGroups();

        for (int i = 0; i < addressableGroups.Count; ++i)
        {
            AddressableAssetGroup addressableAssetGroup = addressableGroups[i];
            if (addressableAssetGroup.Name == groupName)
            {
                return addressableAssetGroup;
            }
        }

        return null;
    }

    public static List<string> GetAddressableLabels()
    {
        return AddressableAssetSettingsDefaultObject.Settings.GetLabels();
    }

    public static void WriteAddressAbleAssetGroupInfoXmlFile()
    {
        string filePath = GetAddressAbleAssetGroupInfoXmlPath();

        //XmlWriterSettings settings = XmlHelper.GetDefaultSetting();
        //using (var xmlWriter = XmlWriter.Create(filePath, settings))
        //{
        //    _assetGroupInfo.Write(xmlWriter);
        //}
    }

    public static void WriteAddressAbleBundleLabelsInfoXmlFile()
    {
        string filePath = GetAddressAbleBundleLabelsInfoXmlPath();
        List<string> addressableLabels = GetAddressableLabels();

        //XmlWriterSettings settings = XmlHelper.GetDefaultSetting();
        //using (var xmlWriter = XmlWriter.Create(filePath, settings))
        //{
        //    xmlWriter.WriteStartElement(AddressableLabelsInfoKey);
        //    for (int i = 0; i < addressableLabels.Count; ++i)
        //    {
        //        XmlHelper.WriteElement(xmlWriter, BundleLabelKey, addressableLabels[i]);
        //    }
        //    xmlWriter.WriteEndElement();
        //}
    }

    public static void InitAssetGroupInfoXmlFile_Simul()
    {
        WriteAddressAbleBundleLabelsInfoXmlFile();

        assetGroupInfo.InitAssetGroupData_Simul();
        WriteAddressAbleAssetGroupInfoXmlFile();

        Debug.Log("InitAssetGroupInfoXmlFile_Simul");
    }
#endif
}
