using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipePrefab;
    public float spawnInterval = 2f;
    public float pipeVariance = 2f;
    private float timer;
    private List<GameObject> pipes = new List<GameObject>();

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnPipe();
            timer = 0;
        }
    }

    public void SpawnPipe()
    {
        float randomSpawnPosY = Random.Range(-pipeVariance, pipeVariance);
        Vector3 spawnPosition = new Vector3(transform.position.x, randomSpawnPosY, 0);
        
        GameObject pipe = Instantiate(pipePrefab, spawnPosition, Quaternion.identity);
        pipes.Add(pipe);
    }

    public void ClearPipes()
    {
        foreach (var pipe in pipes)
        {
            Destroy(pipe);
        }
        pipes.Clear();
    }

    public void ResetSpawner()
    {
        ClearPipes();
        
        timer = 0; // Resetea el temporizador de spawn
    }

    public void StartPipesMovement()
    {
        foreach (var pipe in pipes)
        {
            pipe.GetComponent<Pipe>().enabled = true; // Habilita el movimiento para cada tubo
        }
    }
}
