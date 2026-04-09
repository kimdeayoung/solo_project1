using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StatusInfluenceInfo
{
    private BattleUnit owner;

    private StatusInfluence[] _statusInfluences;
    private int _InfluenceTypeCount;

    public StatusInfluenceInfo(BattleUnit owner)
    {
        this.owner = owner;

        _InfluenceTypeCount = (int)CrowdControlType.Length;
        _statusInfluences = new StatusInfluence[_InfluenceTypeCount];
    }

    public void OnUpdate(float deltaTime)
    {
        for (int i = 0; i < _InfluenceTypeCount; i++)
        {
            if (_statusInfluences[i] != null)
            {
                _statusInfluences[i].OnUpdate(deltaTime);
            }
        }
    }

    public void OnAddStatusInfluence()
    {

    }
}
