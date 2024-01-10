using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEditor.AddressableAssets.Build.Layout.BuildLayout;

public class ObjectPoolManager : Manager<ObjectPoolManager>
{
    private List<PreCreationObject>[] preCreationObjects;
    private int[] activeObjectIndex;

    public override void Init()
    {
        base.Init();

        preCreationObjects = new List<PreCreationObject>[(int)PreCreationObjectType.Length];
        activeObjectIndex = new int[(int)PreCreationObjectType.Length];
    }

    public void CreateObjectAsync(PreCreationObjectData objectData, int createCount = 200)
    {
        int objectTypeIndex = (int)objectData.ObjectType;
        if (preCreationObjects[objectTypeIndex] == null)
        {
            preCreationObjects[objectTypeIndex] = new List<PreCreationObject>(createCount);
        }

        for (int i = 0; i < createCount; ++i)
        {
            AddressableBundleLoader.Instance.InstantiateAsync(objectData.PrefabName, null, (GameObject obj) =>
            {
                PreCreationObject preCreationObject = obj.GetComponent<PreCreationObject>();
                Assert.IsNotNull(preCreationObject);
                preCreationObjects[objectTypeIndex].Add(preCreationObject);
            });
        }
    }

    public PreCreationObject GetUnactiveObject(PreCreationObjectType objectType)
    {
        int objectTypeIndex = (int)objectType;
        Assert.IsNotNull(preCreationObjects[objectTypeIndex]);

        List<PreCreationObject> creationObjects = preCreationObjects[objectTypeIndex];
        int creationObjectCount = creationObjects.Count;
        int objectIndex = activeObjectIndex[objectTypeIndex]++;

        if (activeObjectIndex[objectTypeIndex] > creationObjectCount)
        {
            activeObjectIndex[objectTypeIndex] = 0;
        }
        Assert.IsFalse(creationObjects[objectTypeIndex].IsActive);//해당 assert가 걸리는 경우 object 생성양을 늘려야함
        return creationObjects[objectIndex];
    }

    public override void ClearData()
    {
        for (int i = 0; i < preCreationObjects.Length; ++i)
        {
            List<PreCreationObject> objects = preCreationObjects[i];
            for (int j = 0; j < objects.Count; ++j)
            {
                AddressableBundleLoader.Instance.DestroyInstantiateObj(objects[j].gameObject);
            }

            objects.Clear();
        }
        activeObjectIndex = null;
    }
}
