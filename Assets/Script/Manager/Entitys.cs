using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class Entitys
{
    private Battle battleScene;
    private AddressableBundleLoader loader;
    private List<BattleUnit> activeBattleUnits;

    private Dictionary<string, List<BattleUnit>> _units;

    private int _requestInstantiateCount;
    public bool IsEmptyInstantiate => _requestInstantiateCount == 0;

    public Entitys()
    {
        loader = AddressableBundleLoader.Instance;

        ActionDataPool.Init();

        activeBattleUnits = new List<BattleUnit>(512);
        _units = new Dictionary<string, List<BattleUnit>>(8);

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
                    ++_requestInstantiateCount;
                    loader.InstantiateAsync(data.assetName, battleScene.EntityRoot, OnCreateBattleUnit);
                    break;
            }
        }

        await UniTask.WaitUntil(() => IsEmptyInstantiate);

        battleScene.SetPreLoadState(PreLoadCondition.Entitiy);
    }

    /// <summary>
    /// 게임 내에서 사용할 데이터는 미리 생성하고 들어가게 적용
    /// 가능한 async로 인한 일부 데이터가 안보이는 경우 자체를 아예 없애려면 이러한 방법이 좋아보임..
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public T CreateBattleUnit<T>(string assetName) where T : BattleUnit
    {
        _units.TryGetValue(assetName, out List<BattleUnit> units);
        Debug.Assert(units.Count > 0);

        BattleUnit unit = units[^1];
        Debug.Assert(unit != null);
        units.RemoveAt(units.Count - 1);

        activeBattleUnits.Add(unit);
        unit.OnStart();

        if (units.Count < 8)
        {
            const int createCount = 16;
            _requestInstantiateCount += createCount;
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

        if (!_units.TryGetValue(key, out List<BattleUnit> units))
        {
            int capacity = 0;
            switch (unit.Type)
            {
                case BattleUnitType.Player:
                    capacity = 2;
                    break;
                case BattleUnitType.Enemy:
                    capacity = 256;
                    break;
            }
            units = new List<BattleUnit>(capacity);
            _units.Add(key, units);
        }
        units.Add(unit);

        --_requestInstantiateCount;
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
}

