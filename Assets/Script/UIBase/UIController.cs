using System;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private Transform uiRoot;

    private Dictionary<Type, List<UIBase>> _cachedUis = new Dictionary<Type, List<UIBase>>(8);
    private PriorityQueue<UIBase> _showDelayUis = new PriorityQueue<UIBase>(new UIBaseComparer());
    private List<UIBase> _instantUis = new List<UIBase>(16);

    private UIBase sequenceDisplayUI;

    private int instanceID;
    private uint sequenceID;

    public void Init()
    {
        enabled = false;
    }

    public T Show<T>(AppearPriority priority = AppearPriority.Normal, Action<T> prevAction = null) where T : UIBase
    {
        T uiBase = GetUIBase(typeof(T)) as T;
        prevAction?.Invoke(uiBase);

        uiBase.ResetVariables(priority, sequenceID++);
        uiBase.InvisibleUI();

        _showDelayUis.Enqueue(uiBase);

        enabled = true;

        return uiBase;
    }

    public T ShowInstant<T>(AppearPriority priority = AppearPriority.Normal, Action<T> prevAction = null) where T : UIBase
    {
        T uiBase = GetUIBase(typeof(T)) as T;
        prevAction?.Invoke(uiBase);

        uiBase.ResetVariables(priority, sequenceID++);
        uiBase.VisibleUI();

        _instantUis.Add(uiBase);

        return uiBase;
    }

    public void CloseUI(UIBase uiBase)
    {
        if (sequenceDisplayUI.Equals(uiBase))
        {
            if (_showDelayUis.Count > 0)
            {
                enabled = true;
            }
            else
            {
                sequenceID = 0;
            }
        }
        else
        {
            int instantUiCount = _instantUis.Count;
            for (int i = 0; i < instantUiCount; i++)
            {
                if (_instantUis[i].Equals(uiBase))
                {
                    _instantUis.RemoveAt(i);
                    break;
                }
            }
        }

        uiBase.InvisibleUI();
        _cachedUis.TryGetValue(uiBase.Type, out List<UIBase> uiBases);
        uiBases.Add(uiBase);
    }

    private UIBase GetUIBase(Type type)
    {
        UIBase uiBase = null;
        if (_cachedUis.TryGetValue(type, out List<UIBase> uiBases))
        {
            if (uiBases.Count > 0)
            {
                uiBase = uiBases[0];
                uiBases.RemoveAtSwapBack(0);
            }
            else
            {
                GameObject instance = AddressableBundleLoader.Instance.Instantiate(type.Name, uiRoot);
                uiBase = instance.GetComponent<UIBase>();
                uiBase.Init(instanceID++, type, this);
            }
        }
        else
        {
            _cachedUis.Add(type, new List<UIBase>(4));

            GameObject instance = AddressableBundleLoader.Instance.Instantiate(type.Name, uiRoot);
            uiBase = instance.GetComponent<UIBase>();
            uiBase.Init(instanceID++, type, this);
        }

        return uiBase;
    }

    private void ActiveShowDelayedUI()
    {
        Debug.Assert(sequenceDisplayUI == null);

        if (_showDelayUis.Count > 0)
        {
            _showDelayUis.TryDequeue(out UIBase uiBase);

            sequenceDisplayUI = uiBase;
            uiBase.VisibleUI();

            enabled = false;
        }
    }

    private void Update()
    {
        if (sequenceDisplayUI == null && _showDelayUis.Count > 0)
        {
            ActiveShowDelayedUI();
        }
    }

    public UIBase GetSequenceDisplayUIOrNull()
    {
        return sequenceDisplayUI;
    }

    public UIBase FindFirstInstantUIOrNull<T>() where T : UIBase
    {
        Type type = typeof(T);
        int instantUiCount = _instantUis.Count;
        for (int i = 0; i < instantUiCount; i++)
        {
            if (_instantUis[i].Type == type)
            {
                return _instantUis[i];
            }
        }
        return null;
    }

    public void Release()
    {


        foreach (List<UIBase> uiBases in _cachedUis.Values)
        {
            int loopCount = uiBases.Count;
            for (int i = 0; i < loopCount; i++)
            {
                AddressableBundleLoader.Instance.DestroyInstantiateObj(uiBases[i].gameObject);
            }
        }

        _cachedUis.Clear();
    }
}
