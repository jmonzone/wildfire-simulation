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

public class FuelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> fuelPrefabs;

    private const int DENSITY = 10;

    private void Start()
    {
        InitFuel();
    }

    private void InitFuel()
    {
        var lines = File.ReadLines(@"Assets/Data/fuel.txt").ToArray();

        for (int i = 0; i < lines.Length; i += DENSITY)
        {
            var strings = lines[i].Split(' ').ToList();

            for (int j = 0; j < strings.Count; j += DENSITY)
            {

                if (float.TryParse(strings[j], out var value))
                {
                    if (value >= 0 && value <= 13)
                    {
                        var position = new Vector3(j, 0, i);
                        position.y = Terrain.activeTerrain.SampleHeight(position);

                        var fuelModel = (FuelModel) value;
                        CreateFuelObject(fuelModel, position);
                    }
                }

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
