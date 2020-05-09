using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FireUpdater))]
public class FireSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPool fireObjectPool;
    [SerializeField] private ObjectPool ashObjectPool;

    [Tooltip("Used to customize performance. Use 1 for max performance.")]
    [SerializeField] private int fireDensityLevel;

    private void Awake()
    {
        var fireUpdate = GetComponent<FireUpdater>();
        fireUpdate.OnOutputFileUpdated += OnOutputFileUpdated;
    }

    private void OnOutputFileUpdated()
    {
        ResetFire();
        SpawnFires();
    }

    private void SpawnFires()
    {
        var terrainData = Terrain.activeTerrain.terrainData;

        var lines = File.ReadLines(FireUpdater.OUTPUT_FILE_PATH).ToArray();

        for (int i = 0; i < lines.Length; i += fireDensityLevel)
        {
            var strings = lines[i].Split(' ').ToList();

            for (int j = 0; j < strings.Count; j += fireDensityLevel)
            {
                if (strings[j] == "0") continue;

                var y = terrainData.GetHeight(i, j);
                var spawnPosition = new Vector3(i, y, j);

                if (strings[j] == "1") SpawnFire(spawnPosition);
                else if (strings[j] == "2") SpawnAsh(spawnPosition);
            }
        }
    }

    private void SpawnFire(Vector3 spawnPosition)
    {
        var fireObject = fireObjectPool.GetAvailableObject;
        fireObject.SetActive(true);
        fireObject.transform.position = spawnPosition;
    }

    private void SpawnAsh(Vector3 spawnPosition)
    {
        var ashObject = ashObjectPool.GetAvailableObject;
        ashObject.SetActive(true);
        ashObject.transform.position = spawnPosition;
    }

    private void ResetFire()
    {
        fireObjectPool.DespawnAllObjects();
        ashObjectPool.DespawnAllObjects();
    }
}
