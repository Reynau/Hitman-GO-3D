using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public AudioSource doorAudio;
    public bool initiallyOpen = true;

    public GameObject pivot;

    public float animationTime = 0.5f;
    public float animationDelay = 1f;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutSine;

    bool _open;
    Vector3 openRotation = new Vector3(0f, 90f, 0f);
    Vector3 closeRotation = new Vector3(0f, 0f, 0f);

    float _hzToSecond = 34100f;
    public float soundDelay = 1f;

    private void Start()
    {
        _open = initiallyOpen;
        if (pivot != null)
        {
            if (initiallyOpen) pivot.transform.Rotate(openRotation);
        }
    }

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
                "delay", animationDelay,
                "islocal", true
            ));
            _open = true;
        }
        else
        {
            iTween.RotateTo(pivot, iTween.Hash(
                "rotation", closeRotation,
                "time", animationTime,
                "easetype", easeType,
                "delay", animationDelay,
                "islocal", true
            ));
            _open = false;
        }
        ulong delay = Convert.ToUInt64(soundDelay * _hzToSecond);
        doorAudio.Play(delay);
    }
}
