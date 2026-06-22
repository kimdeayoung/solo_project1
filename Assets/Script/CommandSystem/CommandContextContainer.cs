using System.Collections.Generic;
using System;
using UnityEngine;


[CreateAssetMenu(fileName = "CommandContextContainer", menuName = "Command Context/" + "Command Context Container")]
public class CommandContextContainer : ScriptableObject
{
    [SerializeField] private int identifier;
    public int Identifier => identifier;

    [SerializeField] private BeginCommandConditionType beginConditionType;
    [SerializeReference] private BeginCommandCondition beginCondition;
    
    [SerializeField] private CommandResetFlag resetFlag;
    public CommandResetFlag ResetFlag => resetFlag;

    [SerializeField] private CommandContextNode rootNode;
    private List<CommandContextNode> allNodes; //savepoint Å½»ö¿ë

    [NonSerialized] private bool isInitialize;
    public bool IsInitialize => isInitialize;

    public void Initialize()
    {
        isInitialize = true;

        int loopCount = allNodes.Count;
        for (int i = 0; i < loopCount; i++)
        {
            allNodes[i].Initialize(this);
        }
    }

    public bool IsBeginCommandBehaviour()
    {
        return beginCondition.IsBeginAble(this);
    }

    public void OnStart()
    {
        Debug.Assert(rootNode != null);
        rootNode.OnStart();
    }

    public void RegisterSavePoint(CommandContextNode node)
    {

    }

    public void Stop()
    {
        int loopCount = allNodes.Count;
        for (int i = 0; i < loopCount; i++)
        {
            allNodes[i].Stop();
        }
    }
}
