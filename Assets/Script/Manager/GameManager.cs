using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : ManagerWithMono<GameManager>
{
    private List<IManager> managers;
    private List<IUpdateableManager> updateableManagers;

    public void Awake()
    {
        managers = new List<IManager>(8);
        updateableManagers = new List<IUpdateableManager>(8);

        Tables.Instance.LoadTables_Binary();
    }

    public void RegisterManager(IManager manager)
    {
        Assert.IsFalse(managers.Contains(manager), "No Need Register, Manager Already Contains");

        managers.Add(manager);
    }

    public void UnRegisterManager(IManager manager)
    {
        Assert.IsTrue(managers.Contains(manager), "Manager Not Contains");

        managers.Remove(manager);
    }

    public void RegisterUpdateableManager(IUpdateableManager updateableManager)
    {
        Assert.IsFalse(updateableManagers.Contains(updateableManager), "No Need Register, updateableManager Already Contains : " + updateableManager);

        updateableManagers.Add(updateableManager);
    }

    public void UnRegisterUpdateableManager(IUpdateableManager updateableManager)
    {
        Assert.IsTrue(updateableManagers.Contains(updateableManager), "updateableManager Not Contains");

        updateableManagers.Add(updateableManager);
    }

    public override void ClearData()
    {
        foreach(IManager manager in managers)
        {
            manager.ClearData();
        }
        managers.Clear();

        foreach (IManager updateableManagers in managers)
        {
            updateableManagers.ClearData();
        }
        updateableManagers.Clear();
    }
}
