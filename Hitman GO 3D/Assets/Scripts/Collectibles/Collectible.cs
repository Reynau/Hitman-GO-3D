using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CollectibleType
{
    FastFood,
    HealthyFood
}

public class Collectible : MonoBehaviour
{
    public float delay;

    public bool isPicked = false;

    public UnityEvent pickCollectibleEvent;

    public void PickCollectible()
    {
        if (!isPicked)
        {
            StartCoroutine(PickRoutine());
            isPicked = true;
        }
    }

    IEnumerator PickRoutine()
    {
        yield return new WaitForSeconds(delay);
        pickCollectibleEvent.Invoke();
    }
}
