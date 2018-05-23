using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public GameObject pivot;

    public float animationTime = 0.5f;
    public float animationDelay = 1f;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutSine;

    bool _open = false;
    Vector3 openRotation = new Vector3(0f, 90f, 0f);
    Vector3 closeRotation = new Vector3(0f, 0f, 0f);

	public void Animate ()
    {
        if (pivot == null)
        {
            return;
        }

        if (!_open)
        {
            iTween.RotateTo(pivot, iTween.Hash(
                "rotation", openRotation,
                "time", animationTime,
                "easetype", easeType,
                "delay", animationDelay
            ));
            _open = true;
        }
        else
        {
            iTween.RotateTo(pivot, iTween.Hash(
                "rotation", closeRotation,
                "time", animationTime,
                "easetype", easeType,
                "delay", animationDelay
            ));
            _open = false;
        }
        
    }
}
