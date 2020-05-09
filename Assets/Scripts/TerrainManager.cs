using System.IO;
using System.Linq;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    private void Awake()
    {
        InitHeights();
    }

    private void InitHeights()
    {
        var terrainData = Terrain.activeTerrain.terrainData;
        var heightmapResolution = terrainData.heightmapResolution;
        var heights = new float[heightmapResolution, heightmapResolution];

        var lines = File.ReadLines(@"Assets/Data/slope.txt").ToArray();

        for (int i = 0; i < lines.Length; i++)
        {
            var strings = lines[i].Split(' ').ToList();

            for (int j = 0; j < strings.Count; j++)
            {
                if (float.TryParse(strings[j], out var y))
                {
                    y = y == -9999 ? 0 : y;
                    heights[i, j] = y / heightmapResolution;
                }

            }
        }
            
        terrainData.SetHeights(0, 0, heights);

        Debug.Log($"Terrain heights initialized.");
    }
}
