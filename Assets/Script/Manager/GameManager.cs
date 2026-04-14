using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameManager : ManagerWithMono<GameManager>
{
    private List<IManager> managers;
    private List<IUpdateableManager> updateableManagers;

    public GlobalVariables GlobalVariables { get; private set; }

    private SceneInstance sceneInstance;
    private Dictionary<string, System.Type> sceneTypes = new Dictionary<string, System.Type>(8);

    public SceneInstance SceneInstance() => sceneInstance;
    public T SceneInstance<T>() where T : SceneInstance => sceneInstance as T;

    public void Awake()
    {
        managers = new List<IManager>(8);
        updateableManagers = new List<IUpdateableManager>(8);

        GlobalVariables = Resources.Load<GlobalVariables>("ScriptableObject/GlobalVariables");

        AddressableBundleLoader.Instance.InitInstance();
        DontDestroyOnLoad(this);

        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    private void ActiveSceneChanged(Scene current, Scene next)
    {
        if (sceneInstance != null)
        {
            sceneInstance.Release();
        }

        System.Type sceneType;
        if (!sceneTypes.TryGetValue(next.name, out sceneType))
        {
            sceneType = System.Type.GetType(next.name);
            Debug.Assert(sceneType != null);

            sceneTypes.Add(next.name, sceneType);
        }
        sceneInstance = new GameObject(next.name, sceneType).GetComponent<SceneInstance>();
        sceneInstance.PreLoad();
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
