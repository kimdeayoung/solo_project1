using System.Collections.Generic;
using UnityEngine;

public static class PlayData
{
    private static List<CreateInAdvanceData> createInAdvanceData = new List<CreateInAdvanceData>(32);
    public static IReadOnlyList<CreateInAdvanceData> CreateInAdvanceData => createInAdvanceData;

    public static void InsertAdvanceData(CreateInAdvanceData data)
    {
        createInAdvanceData.Add(data);
    }

    public static void ClearAdvanceData()
    {
        createInAdvanceData.Clear();
    }

    public static void Clear()
    {
        createInAdvanceData.Clear();
    }
}

public readonly struct CreateInAdvanceData
{
    public readonly string assetName;
    public readonly int createCount;

    public readonly ObjectType objectType;

    public CreateInAdvanceData(string assetName, int createCount, ObjectType objectType)
    {
        this.assetName = assetName;
        this.createCount = createCount;
        this.objectType = objectType;
    }
}