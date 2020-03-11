using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum FuelModel
{
    SHORT_GRASS = 0,
    GRASS_WITH_TIMBER = 1,
    MATURE_BRUSH = 4,
    YOUNG_BRUSH = 5,
    INTERMEDIATE_BRUSH = 6,
    CLOSED_LITTER = 8,
    HARDWOOD_LITTER = 9,
    MATURE_TIMBER = 10,
    LIGHT_SLASH = 11,
    MEDIUM_SLASH = 12,
    HEAVY_SLASH = 13
}

public class FuelGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> fuelPrefabs;

    [Header("Options")]
    [SerializeField] private int densityLevel = 20;

    private const float SCALE_FACTOR = 4f;
    private const float OFFSET_X = -(512 - 382) / 2 * SCALE_FACTOR + 2048;
    private const float OFFSET_Z = (512 - 266) / 2 * SCALE_FACTOR;


    private void Start()
    {
        var fuelPositions = GetFuelPositions();
        CreateFuelObjects(fuelPositions);
    }

    private Dictionary<FuelModel, List<Vector3>> GetFuelPositions()
    {
        var fuelPositions = new Dictionary<FuelModel, List<Vector3>>();

        var j = 0;
        foreach (string line in File.ReadLines(@"Assets/Terrain/fuel.txt"))
        {
            if (j % densityLevel != 0)
            {
                j++;
                continue;
            }

            var strings = line.Split(' ').ToList();

            var i = 0;
            foreach (string str in strings)
            {
                if (i % densityLevel != 0)
                {
                    i++;
                    continue;
                }

                if (int.TryParse(str, out var value))
                {
                    if (value >= 0 && value <= 13)
                    {
                        var x = -i * SCALE_FACTOR + OFFSET_X + Random.Range(-5, 5);
                        var z = j * SCALE_FACTOR + OFFSET_Z + Random.Range(-5, 5);

                        var position = new Vector3(x, 0, z);
                        position.y = Terrain.activeTerrain.SampleHeight(position);

                        var fuelModel = (FuelModel)value;
                        if (!fuelPositions.ContainsKey(fuelModel)) fuelPositions.Add(fuelModel, new List<Vector3>());
                        fuelPositions[fuelModel].Add(position);
                    }

                    i++;
                }
            }

            j++;
        }
        return fuelPositions;
    }


    private void CreateFuelObjects(Dictionary<FuelModel, List<Vector3>> fuelPositions)
    {
        foreach (FuelModel fuelModel in fuelPositions.Keys)
        {
            foreach (Vector3 position in fuelPositions[fuelModel])
            {
                CreateFuelObject(fuelModel, position);
            }
        }
    }

    private GameObject CreateFuelObject(FuelModel fuelModel, Vector3 position)
    {
        var fuelObject = Instantiate(fuelPrefabs[0], position, Quaternion.identity, transform);
        fuelObject.name = fuelModel.ToString();
        return fuelObject;
    }

}
