using System;
using System.Collections;
using UnityEngine;

public struct OnFireSpreadEventArgs
{
    public FireBehaviour fire;
}

public class FireBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem fire;

    [Header("Options")]
    [SerializeField] private float rateOfSpread;
    [SerializeField] private float flameLength;

    public event Action<OnFireSpreadEventArgs> OnFireSpread;
    private bool onFire = false;

    private void Awake()
    {
        var col = GetComponent<SphereCollider>();
        //col.radius = flameLength * 20;
    }

    private void OnMouseDown()
    {
        Debug.Log($"OnMouseDown called on {gameObject}. Catching on fire.");
        CatchOnFire();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider entered trigger.");
        var fire = other.GetComponent<FireBehaviour>();
        if (fire)
        {
            fire.OnFireSpread += OnFireSpreadEventListener;
        }
    }

    private void OnFireSpreadEventListener(OnFireSpreadEventArgs args)
    {
        Debug.Log("Nearby fire spread.");
        CatchOnFire();
        args.fire.OnFireSpread -= OnFireSpreadEventListener;
    }

    private void CatchOnFire()
    {
        if (!onFire)
        {
            onFire = true;
            fire.gameObject.SetActive(true);
            fire.Play();
            var main = fire.main;
            main.startSize = flameLength * 10;
            StartCoroutine(FireUpdate());
        }
    }

    private IEnumerator FireUpdate()
    {
        var rate = FireManager.Instance.TestMode ? FireManager.Instance.RateOfSpread : rateOfSpread * 3600.0f;

        while (enabled)
        {
            yield return new WaitForSeconds(rate);
            OnFireSpread?.Invoke(new OnFireSpreadEventArgs()
            {
                fire = this
            }); ;
        }
        
    }
}
