using System.Collections;
using System.Collections.Generic;

public abstract class UnitStatus
{
    protected BattleUnit owner;

    protected int hp;
    protected int maxHp;

    protected uint atk;
    protected uint def;

    protected float moveSpeed;

    protected StatusInfluenceInfo influenceInfo;

    public UnitStatus(BattleUnit owner)
    {
        this.owner = owner;
        influenceInfo = new StatusInfluenceInfo(owner);
    }

    public void Update()
    {
        influenceInfo.Update();
    }

    public bool IsAlive()
    {
        return hp > 0;
    }
}
