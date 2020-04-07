using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    private static FireManager instance = null;
    public static FireManager Instance => instance;

    [Header("Options")]
    [SerializeField] private bool testMode;
    [SerializeField] private float rateOfSpread;

    public bool TestMode => testMode;
    public float RateOfSpread => rateOfSpread;

    private void Awake()
    {
        instance = this;
    }
}
