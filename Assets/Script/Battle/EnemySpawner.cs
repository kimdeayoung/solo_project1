using UnityEngine;

public class EnemySpawner
{
    private Player player;
    private Entitys entitys;
    private float term;

    public void Init()
    {
        entitys = GameManager.Instance.SceneInstance<Battle>().Entity;
        term = 2.0f;
    }

    public void RegisterPlayer(Player player)
    {
        this.player = player;
    }

    public void OnUpdate(float deltaTime)
    {
        term -= deltaTime;

        if (term < 0)
        {
            Vector3 playerPos = player.transform.position;

            for (int i = 0; i < 3; i++)
            {
                Vector2 randomDir = Random.insideUnitCircle;

                Enemy enemy = entitys.CreateBattleUnit<Enemy>("Golem_1");
                enemy.transform.position = playerPos + new Vector3(playerPos.x + randomDir.x * 5f, playerPos.y, playerPos.z + randomDir.y * 5f);
            }

            term = 100000000000.0f;
        }
    }
}
