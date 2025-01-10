using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy prefab;
    public EnemyData[] datas;
    public Transform[] spawinPoints;

    private int wave = 0;
    private List<Enemy> zombies = new List<Enemy>();

    private GameManager gm;

    private void Start()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (gm.IsGameOver)
            return;

        if (zombies.Count == 0)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        ++wave;
        int count = Mathf.RoundToInt(wave * 1.5f);
        for (int i = 0; i < count; ++i)
        {
            CreateZombie();
        }
    }

    private void CreateZombie()
    {
        var data = datas[Random.Range(0, datas.Length)];
        var spawnPoint = spawinPoints[Random.Range(0, spawinPoints.Length)];

        var zombie = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        zombie.Setup(data);
        zombies.Add(zombie);

        zombie.onDeath += () => zombies.Remove(zombie);
        zombie.onDeath += () => Destroy(zombie.gameObject, 5f);
        zombie.onDeath += () => gm.AddScore(100);
        //zombie.onDeath += () => gm.uiManager.UpdateWaveText(wave, zombies.Count);

       // gm.uiManager.UpdateWaveText(wave, zombies.Count);
    }
}
