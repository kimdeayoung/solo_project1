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

        SceneManager.LoadScene("Battle");
    }
}
