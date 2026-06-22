using System;
using UnityEngine;

[Serializable]
public abstract class CommandContext
{
    [SerializeField] private CommandContextFlag contextFlag;

    private bool isExit;
    protected CommandContextNode contextNode;

    public abstract CommandContextType CommandContextType { get; }

    public void Initialize(CommandContextNode node)
    {
        contextNode = node;
    }

    public void OnEnterContext()
    {
        isExit = false;
        OnEnter();

        if (HasContextFlag(CommandContextFlag.MoveNextImmediate))
        {
            MoveNext();
        }
    }

    protected void MoveNext()
    {
        if (isExit)
        {
            return;
        }

        isExit = true;
        contextNode.MoveNext();
    }

    protected void Undo(int count)
    {
        contextNode.Undo(count);
    }

    protected abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnStop();

    public bool HasContextFlag(CommandContextFlag flag)
    {
        return (contextFlag & flag) == flag;
    }
}
