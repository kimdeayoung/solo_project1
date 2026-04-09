using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StatusInfluenceInfo
{
    private BattleUnit owner;

    private List<StatusInfluence> _statusInfluences;

    public StatusInfluenceInfo(BattleUnit owner)
    {
        this.owner = owner;

        _statusInfluences = new List<StatusInfluence>(16);
    }

    public void OnUpdate(float deltaTime)
    {
        int influenceTypeCount = _statusInfluences.Count;
        for (int i = 0; i < influenceTypeCount;)
        {
            if (_statusInfluences[i] != null)
            {
                bool isRemoved = _statusInfluences[i].OnUpdate(deltaTime);
                if (isRemoved)
                {
                    _statusInfluences.RemoveAtSwapBack(i);
                }
                else
                {
                    ++i;
                }
            }
        }
    }

    public void OnAddStatusInfluence()
    {

    }
}
