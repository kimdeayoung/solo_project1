using System;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

[RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
public class UIBase : MonoBehaviour, IEquatable<UIBase>
{
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected CanvasGroup canvasGroup;

    [SerializeField] private SafeArea safeArea;

    public int InstanceID { get; private set; }

    public Type Type { get; private set; }
    private UIController controller;

    public AppearPriority AppearPriority { get; private set; }
    public uint SequenceID { get; private set; }

    public void Init(int instanceID, Type type, UIController controller)
    {
        InstanceID = instanceID;
        Type = type;
        this.controller = controller;
    }

    public void ResetVariables(AppearPriority appearPriority, uint sequenceID)
    {
        AppearPriority = appearPriority;
        SequenceID = SequenceID;
    }

    public virtual void VisibleUI()
    {
        safeArea.OnVisible();
    }

    public virtual void InvisibleUI()
    {
        safeArea.OnInvisible();
    }

    public virtual void Close()
    {
        
    }

    public bool Equals(UIBase other)
    {
        return InstanceID == other.InstanceID;
    }

    public override bool Equals(object obj) => Equals(obj as UIBase);
    public override int GetHashCode() => InstanceID;
}

public class UIBaseComparer : IComparer<UIBase>
{
    public int Compare(UIBase x, UIBase y)
    {
        return x.AppearPriority - y.AppearPriority;
    }
}
