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
    ActionObject,
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
    Action,
    Dead,
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
    Knockback,
    Invincible,
    Length,
}

public enum AddStatusInfluenceType
{
    Independent,
    Stack,
    Unique,
}

public enum AppearPriority
{
    VeryHigh,
    High,
    Normal,
    Low,
    VeryLow,
}