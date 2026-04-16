public enum TableType
{
    Stage,
    Length,
}

public enum TableLoadType
{
    Local,
    Binary,
}

public enum SearchMethodType
{
    Self,
    ByDistance,
    InRange,
    Length,
}

public enum ObjectType
{
    BattleUnit,
    Projectile,
    Length,
}

public enum BattleStateType
{
    BattleInit,
    Progress,
    Victory,
    Defeat,
}

public enum UnitState
{
    Idle,
    Stun,
    Chase,
    Action,
}

public enum StageDifficultyType
{
    Normal,
}

public enum PreLoadCondition
{
    LoadAsset,
    Entitiy,
    Length,
}

public enum PreLoadState
{
    NotReady,
    Ready,
}

public enum SkillType
{
    Attack,
    Buff,
    DeBuff,
    CrowdControl,
    CreateProjectile,
}

public enum EventType
{
    NormalAttack,
    CreateProjectile,
}

public enum StatusInfluenceType
{
    Stun,
    MoveSpeedUp,
    MoveSpeedDown,
    Length,
}

public enum AddStatusInfluenceType
{
    Independent,
    Stack,
}

public enum AppearPriority
{
    VeryHigh,
    High,
    Normal,
    Low,
    VeryLow,
}