using System.Collections.Generic;
using System.Diagnostics;

public class StatusInfluenceInfo
{
    private WorldObject owner;

    private List<StatusInfluence> _statusInfluences;
    private Entitys entitys;

    public StatusInfluenceInfo(WorldObject owner)
    {
        this.owner = owner;

        _statusInfluences = new List<StatusInfluence>(16);
        entitys = GameManager.Instance.SceneInstance<Battle>().Entity;
    }

    public void OnUpdate(float deltaTime)
    {
        int influenceTypeCount = _statusInfluences.Count;
        for (int i = 0; i < influenceTypeCount;)
        {
            bool isRemoved = _statusInfluences[i].OnUpdate(deltaTime);
            if (isRemoved)
            {
                StatusInfluence statusInfluence = _statusInfluences[i];
                entitys.ReleaseStatusInfluence(statusInfluence);

                _statusInfluences.RemoveAtSwapBack(i);
                --influenceTypeCount;
            }
            else
            {
                ++i;
            }
        }
    }

    public void ApplyStatusInfluence(AddStatusInfluenceData data)
    {
        Debug.Assert(entitys != null);

        switch (data.addStatusInfluenceType)
        {
            case AddStatusInfluenceType.Stack:
                {
                    StatusInfluence preStatusInfluence = FindStatusInfluenceOrNull(data.statusInfluenceType, data.addStatusInfluenceType);
                    if (preStatusInfluence != null)
                    {
                        preStatusInfluence.AddInfluence(data);
                        return;
                    }
                }
                break;
        }

        StatusInfluence statusInfluence = entitys.GetStatusInfluence(data.statusInfluenceType);
        statusInfluence.OnStart(owner, data);
    }

    public StatusInfluence FindStatusInfluenceOrNull(StatusInfluenceType statusInfluenceType, AddStatusInfluenceType addStatusInfluenceType)
    {
        int influenceTypeCount = _statusInfluences.Count;
        for (int i = 0; i < influenceTypeCount; i++)
        {
            StatusInfluence statusInfluence = _statusInfluences[i];
            if (statusInfluence.InfluenceType == statusInfluenceType && statusInfluence.AddStatusInfluenceType == addStatusInfluenceType)
            {
                return statusInfluence;
            }
        }
        return null;
    }

    public void FindStatusInfluences(StatusInfluenceType statusInfluenceType, AddStatusInfluenceType addStatusInfluenceType, List<StatusInfluence> statusInfluences)
    {
        int influenceTypeCount = _statusInfluences.Count;
        for (int i = 0; i < influenceTypeCount; i++)
        {
            StatusInfluence statusInfluence = _statusInfluences[i];
            if (statusInfluence.InfluenceType == statusInfluenceType && statusInfluence.AddStatusInfluenceType == addStatusInfluenceType)
            {
                statusInfluences.Add(statusInfluence);
            }
        }
    }
}
