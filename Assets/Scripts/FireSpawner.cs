using System.IO;
using System.Linq;
using UnityEngine;

//READS the file from FireUpdater.OUTPUT_FILE_PATH, and spawns fire objects
[RequireComponent(typeof(FireUpdater))]
public class FireSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPool fireObjectPool;
    [SerializeField] private ObjectPool ashObjectPool;

    [Tooltip("Used to customize the density of fire spawned. Use 1 for high density, 10 for low density.")]
    [Range(1, 10)]
    [SerializeField] private int fireDensityLevel = 3;

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
