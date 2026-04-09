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
}
