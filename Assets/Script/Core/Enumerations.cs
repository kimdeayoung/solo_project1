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

public enum PreCreationObjectType
{
    Length,
}

public enum BattleStateType
{
    BattleInit,
    Progress,
    Victory,
    Defeat,
}

public enum UnitType
{
    Character,

}

public enum UnitState
{
    Idle,
    Stun,
}

public enum StageDifficultyType
{
    Normal,
}

public enum LoadState
{
    NotComplete,
    Complete,
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
    Slow,
    Length,
}

public enum AddStatusInfluenceType
{
    Independent,
    Stack,
}