using System;
using System.Collections;
using System.IO;
using UnityEngine;


//Calls a method to set the values of an output txt file on every interval after being toggled on
//Currently being toggled by the FireStarter.cs file on Mouse Button click
public class FireUpdater : MonoBehaviour
{
    [Tooltip("A float value determing how often the fire output should be updated. (IN SECONDS)")]
    [SerializeField] private float interval = 2.0f;

    public const int MATRIX_ROWS = 382;
    public const int MATRIX_COLS = 266;
    public const string OUTPUT_FILE_PATH = @"Assets/Data/output.txt";

    private bool isUpdating = false;

    public event Action OnOutputFileUpdated;

    public void ToggleUpdate()
    {
        isUpdating = !isUpdating;
        Debug.Log("Fire is updating: " + isUpdating.ToString());

        if (isUpdating) StartCoroutine(FireUpdate());

    }

    private IEnumerator FireUpdate()
    {
        while (isUpdating)
        {
            UpdateFireOutputFile();
            yield return new WaitForSeconds(interval);
        }
    }

    private void UpdateFireOutputFile()
    {
        string[] output = new string[MATRIX_ROWS];

        for (int i = 0; i < MATRIX_ROWS; i++)
        {
            output[i] = "";

            for (int j = 0; j < MATRIX_COLS; j++)
            {
                var value = UnityEngine.Random.Range(0, 3);
                output[i] += value.ToString() + " ";
            }
        }

        File.WriteAllLines(OUTPUT_FILE_PATH, output);
        OnOutputFileUpdated?.Invoke();
        Debug.Log("Output file updated.");
    }
}
