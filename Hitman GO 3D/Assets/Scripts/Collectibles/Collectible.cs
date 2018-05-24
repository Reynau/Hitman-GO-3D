using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectible : MonoBehaviour
{

    public float delay;

    protected bool _picked = false;

    public UnityEvent pickCollectibleEvent;

    public void PickCollectible()
    {
        Debug.Log("CALL ROUTINE PICK COLLECTIBLE! ==================");
        Debug.Log(_picked);
        if (!_picked)
        {
            StartCoroutine(PickRoutine());
            _picked = true;
        }
    }

    IEnumerator PickRoutine()
    {
        yield return new WaitForSeconds(delay);
        pickCollectibleEvent.Invoke();
    }
}
