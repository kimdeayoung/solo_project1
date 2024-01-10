using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PreCreationObjectData
{
    private PreCreationObjectType objectType;
    private string prefabName;

    public PreCreationObjectType ObjectType { get => objectType; }
    public string PrefabName { get => prefabName; }

    public PreCreationObjectData(PreCreationObjectType objectType, string prefabName)
    {
        this.objectType = objectType;
        this.prefabName = prefabName;
    }
}

public abstract class PreCreationObject : MonoBehaviour
{
    protected PreCreationObjectType objectType;
    protected bool isActive;

    public PreCreationObjectType ObjectType { get => objectType; }
    public bool IsActive { get => isActive; }

    public virtual void Init(PreCreationObjectData objData)
    {
        objectType = objData.ObjectType;
    }

    public void Awake()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    public virtual void OnActive()
    {
        gameObject.SetActive(true);
        isActive = true;
    }

    public void Release()
    {
        gameObject.SetActive(false);
        isActive = false;
    }
}
