using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sniper : MonoBehaviour
{

    public float delay;

    public bool isPicked = false;

    public UnityEvent pickSniperEvent;

    public void PickSniper()
    {
        if (!isPicked)
        {
            StartCoroutine(PickSniperRoutine());
            isPicked = true;
        }
    }

    IEnumerator PickSniperRoutine()
    {
        yield return new WaitForSeconds(delay);
        pickSniperEvent.Invoke();
    }
}
