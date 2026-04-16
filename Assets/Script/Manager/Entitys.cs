using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class Entitys
{
    private Battle battleScene;
    private AddressableBundleLoader loader;
    private List<BattleUnit> activeBattleUnits;

    private BattleUnit player;
    private Dictionary<string, List<BattleUnit>> units;

    private List<SearchMethod>[] searchMethodPool;

    private List<StatusInfluence>[] statusInfluencePool;

    private int requestInstantiateCount;
    public bool IsEmptyInstantiate => requestInstantiateCount == 0;

    public Entitys()
    {
        loader = AddressableBundleLoader.Instance;

        ActionDataPool.Init();
        ActionParameterPool.Init();

        activeBattleUnits = new List<BattleUnit>(512);
        units = new Dictionary<string, List<BattleUnit>>(8);

        {
            int searchMethodTypeCount = (int)SearchMethodType.Length;
            searchMethodPool = new List<SearchMethod>[searchMethodTypeCount];

            for (int i = 0; i < searchMethodTypeCount; i++)
            {
                searchMethodPool[i] = new List<SearchMethod>(32);
            }
        }

        {
            int statusInfluenceTypeCount = (int)StatusInfluenceType.Length;
            statusInfluencePool = new List<StatusInfluence>[statusInfluenceTypeCount];

            for (int i = 0; i < statusInfluenceTypeCount; i++)
            {
                statusInfluencePool[i] = new List<StatusInfluence>(32);
            }
        }

        battleScene = GameManager.Instance.SceneInstance<Battle>();
        Debug.Assert(battleScene != null);
    }

    public async UniTask Initialize()
    {
        IReadOnlyList<CreateInAdvanceData> createInAdvanceData = PlayData.CreateInAdvanceData;
        int loopCount = createInAdvanceData.Count;

        for (int i = 0; i < loopCount; i++)
        {
            CreateInAdvanceData data = createInAdvanceData[i];
            switch (data.objectType)
            {
                case ObjectType.BattleUnit:
                    ++requestInstantiateCount;
                    loader.InstantiateAsync(data.assetName, battleScene.EntityRoot, OnCreateBattleUnit);
                    break;
            }
        }

        await UniTask.WaitUntil(() => IsEmptyInstantiate);

        activeBattleUnits.Add(player);
        player.OnStart();

        battleScene.SetPreLoadState(PreLoadCondition.Entitiy);
    }

    public void OnUpdate(float deltaTime)
    {
        int loopCount = activeBattleUnits.Count;
        for (int i = 0; i < loopCount; i++)
        {
            activeBattleUnits[i].OnUpdate(deltaTime);
        }
    }

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        int loopCount = activeBattleUnits.Count;
        for (int i = 0; i < loopCount; i++)
        {
            activeBattleUnits[i].OnFixedUpdate(fixedDeltaTime);
        }
    }

    #region BattleUnit
    /// <summary>
    /// 게임 내에서 사용할 데이터는 미리 생성하고 들어가게 적용
    /// 가능한 async로 인한 일부 데이터가 안보이는 경우 자체를 아예 없애려면 이러한 방법이 좋아보임..
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public T CreateBattleUnit<T>(string assetName) where T : BattleUnit
    {
        this.units.TryGetValue(assetName, out List<BattleUnit> units);
        Debug.Assert(units.Count > 0);

        BattleUnit unit = units[^1];
        Debug.Assert(unit != null);
        units.RemoveAt(units.Count - 1);
        unit.gameObject.SetActive(true);

        activeBattleUnits.Add(unit);
        unit.OnStart();

        if (units.Count < 8)
        {
            const int createCount = 16;
            requestInstantiateCount += createCount;
            for (int i = 0; i < createCount; i++)
            {
                loader.InstantiateAsync(assetName, battleScene.EntityRoot, OnCreateBattleUnit);
            }
        }

        return unit as T;
    }

    private void OnCreateBattleUnit(string key, GameObject obj)
    {
        BattleUnit unit = obj.GetComponent<BattleUnit>();
        unit.Init(key);

        List<BattleUnit> units = null;
        switch (unit.Type)
        {
            case BattleUnitType.Player:
                player = unit;
                break;
            case BattleUnitType.Enemy:
                if (!this.units.TryGetValue(key, out units))
                {
                    int capacity = 256;
                    units = new List<BattleUnit>(capacity);
                    this.units.Add(key, units);
                }
                unit.gameObject.SetActive(false);
                units.Add(unit);
                break;
        }
        --requestInstantiateCount;
    }

    public BattleUnit FindBattleUnitOrNull(BattleUnitType type)
    {
        switch (type)
        {
            case BattleUnitType.Player:
                return player;

            case BattleUnitType.Length:
                return null;
        }

        int loopCount = activeBattleUnits.Count;
        for (int i = 0; i < loopCount; i++)
        {
            BattleUnit unit = activeBattleUnits[i];
            if (unit.Type == type)
            {
                return unit;
            }
        }

        return null;
    }
    #endregion

    #region StatusInfluence
    public StatusInfluence GetStatusInfluence(StatusInfluenceType type)
    {
        StatusInfluence statusInfluence = null;
        List<StatusInfluence> statusInfluences = statusInfluencePool[(int)type];
        int count = statusInfluences.Count;
        if (count > 0)
        {
            statusInfluence = statusInfluences[^1];
            statusInfluences.RemoveAt(count);
        }
        else
        {
            statusInfluence = CreateStatusInfluence(type);
        }

        return statusInfluence;
    }

    public void ReleaseStatusInfluence(StatusInfluence statusInfluence)
    {
        List<StatusInfluence> statusInfluences = statusInfluencePool[(int)statusInfluence.InfluenceType];
        statusInfluences.Add(statusInfluence);
    }

    private StatusInfluence CreateStatusInfluence(StatusInfluenceType type)
    {
        switch (type)
        {
            case StatusInfluenceType.Stun:
                return new Stun();

            case StatusInfluenceType.MoveSpeedUp:
                return new MoveSpeedUp();

            case StatusInfluenceType.MoveSpeedDown:
                return new MoveSpeedDown();
        }

        return null;
    }
    #endregion

    #region SearchMethod
    public SearchMethod GetSearchMethod(SearchMethodType type, SearchMethodProperty data)
    {
        SearchMethod searchMethod = null;
        List<SearchMethod> searchMethods = searchMethodPool[(int)type];
        int count = searchMethods.Count;
        if (count > 0)
        {
            searchMethod = searchMethods[^1];
            searchMethods.RemoveAt(count);
        }
        else
        {
            searchMethod = CreateSearchMethod(type);
        }
        searchMethod.ResetVariables(data);

        return searchMethod;
    }

    public void ReleaseSearchMethod(SearchMethod searchMethod)
    {
        List<SearchMethod> searchMethods = searchMethodPool[(int)searchMethod.Type];
        searchMethods.Add(searchMethod);
    }

    private SearchMethod CreateSearchMethod(SearchMethodType type)
    {
        switch (type)
        {
            case SearchMethodType.Self:
                return new Self();
            case SearchMethodType.ByDistance:
                return new ByDistance();
        }

        return null;
    }
    #endregion
}

