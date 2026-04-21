using UnityEngine.SceneManagement;

public class Title : SceneInstance
{
    public override void PreLoad()
    {
        base.PreLoad();

        OnStart();
    }

    protected override void OnStart()
    {
        base.OnStart();

        PlayData.InsertAdvanceData(new CreateInAdvanceData("Player", 1, ObjectType.BattleUnit));
        PlayData.InsertAdvanceData(new CreateInAdvanceData("Golem_1", 32, ObjectType.BattleUnit));
        PlayData.InsertAdvanceData(new CreateInAdvanceData("ActionObject", 64, ObjectType.ActionObject));

        SceneManager.LoadScene("Battle");
    }
}
