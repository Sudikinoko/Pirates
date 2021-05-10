using System.Collections.Generic;
using UnityEngine;

public class AISpawnManager : MonoBehaviour
{

    public static AISpawnManager instance;

    public Transform homebase;
    public float distancePerLevel = 300f;
    public int instancesPerLevel = 50;

    public float spawnInterval = 10f;
    public float minSpawnRange = 1000f;
    public float maxSpawnRange = 2000f;
    public float despawnRange = 5000f;

    public List<GameObject> militarySpawn;
    public List<GameObject> cargoSpawn;

    PlayerController playerController;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.instance;
        InvokeRepeating("ManageSpawns", 0f, spawnInterval);
    }

    void ManageSpawns()
    {
        float distanceToHomeBase = (playerController.transform.position - homebase.position).magnitude;

        int level = (int)(distanceToHomeBase / distancePerLevel) + 1;

        GameObject[] enemies = TargetFinder.FindEnemies(new List<string> { "Enemy" });

        int amountToSpawn = instancesPerLevel * level - enemies.Length;

        Debug.Log("Spawn " + amountToSpawn + " new Enemies");


        for (int i = 0; i < amountToSpawn; i++)
        {
            //TODO Playerlevel/Ansehen einbeziehen
            Vector3 spawnPosition;
            do
            {
                spawnPosition = playerController.transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(minSpawnRange, maxSpawnRange);
            }
            while (Physics.Raycast(spawnPosition + Vector3.up * 1000f, Vector3.down, 1000f, 3));

            Instantiate(cargoSpawn[0], new Vector3(spawnPosition.x, 0f, spawnPosition.z), Quaternion.identity);
        }

        foreach (GameObject enemy in enemies)
        {
            float distance = (enemy.transform.position - playerController.transform.position).magnitude;
            if (distance > maxSpawnRange)
            {
                Destroy(enemy);
            }
        }
    }
}
