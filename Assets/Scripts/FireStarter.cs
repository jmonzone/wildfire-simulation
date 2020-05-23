using UnityEngine;

//Controller class to toggle fire update
[RequireComponent(typeof(FireUpdater))]
public class FireStarter : MonoBehaviour
{
    private FireUpdater fireUpdate;

    private void Awake()
    {
        fireUpdate = GetComponent<FireUpdater>();
    }

    private void Update()
	{
        if (Input.GetMouseButtonDown(0))
        {
            ToggleFire();
        }
	}

    private void ToggleFire()
    {
        fireUpdate.ToggleUpdate();
    }

    
}
