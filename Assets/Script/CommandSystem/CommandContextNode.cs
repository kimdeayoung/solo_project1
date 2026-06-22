using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommandContextNode", menuName = "Command Context/" + "Command Context Node")]
public class CommandContextNode : ScriptableObject
{
    [SerializeReference] private List<CommandContext> _commandContexts = new List<CommandContext>(8);
    [SerializeField] private bool _requireSave;

    [NonSerialized] private CommandContextContainer _commandContextContainer;

    [NonSerialized] private int _contextIndex;
    public int ContextIndex => _contextIndex;

    public void Initialize(CommandContextContainer container)
    {
        _commandContextContainer = container;

        int loopCount = _commandContexts.Count;
        for (int i = 0; i < loopCount; i++)
        {
            _commandContexts[i].Initialize(this);
        }
    }

    public void OnStart()
    {
        _contextIndex = 0;
        _commandContexts[_contextIndex].OnEnterContext();

        if (_requireSave)
        {
            _commandContextContainer.RegisterSavePoint(this);
        }
    }

    public void Undo(int count)
    {
        _commandContexts[_contextIndex].OnStop();

        _contextIndex -= count;
        Debug.Assert(_contextIndex >= 0 && _commandContexts.Count > _contextIndex);
        _commandContexts[_contextIndex].OnEnterContext();
    }

    public void MoveNext()
    {
        _commandContexts[_contextIndex].OnExit();

        if (_commandContexts.Count > ++_contextIndex)
        {
            _commandContexts[_contextIndex].OnEnterContext();
        }
    }

    public void Stop()
    {
        int loopCount = _commandContexts.Count;
        for (int i = 0; i < loopCount; i++)
        {
            _commandContexts[i].OnStop();
        }
    }
}
