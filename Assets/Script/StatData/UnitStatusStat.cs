using UnityEngine;

public abstract class UnitStat : ScriptableObject
{
    [SerializeField] private int _hp;
    public int Hp => _hp;

    [SerializeField] private float _atk;
    public float Atk => _atk;

    [SerializeField] private float _def;
    public float Def => _def;

    [SerializeField] private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;

    [SerializeField] private float _rotate;
    public float Rotate => _rotate;

    [SerializeField] private BaseActionDataSO[] _collisionActions = System.Array.Empty<BaseActionDataSO>();
    public BaseActionDataSO[] CollisionActions => _collisionActions;

    [SerializeField] private BaseActionDataSO[] _actionDatas = System.Array.Empty<BaseActionDataSO>();
    public BaseActionDataSO[] ActionDatas => _actionDatas;
}
