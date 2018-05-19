using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(MaskableGraphic))]
public class ScreenFader : MonoBehaviour {
    public Color solidColor = new Color(1f, 1f, 1f, 1f);
    public Color clearColor = new Color(1f, 1f, 1f, 0f);

    public float delay = 0.5f;
    public float timeToFade = 1f;
    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

    MaskableGraphic graphic;

    void Awake()
    {
        graphic = GetComponent<MaskableGraphic>();
    }

    void UpdateColor (Color newColor)
    {
        graphic.color = newColor;
    }

    public void FadeOff ()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", solidColor,
            "to", clearColor,
            "easetype", easeType,
            "time", timeToFade,
            "delay", delay,
            "onupdatetarget", gameObject,
            "onupdate", "UpdateColor"
        ));
    }

    public void FadeOn()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", clearColor,
            "to", solidColor,
            "easetype", easeType,
            "time", timeToFade,
            "delay", delay,
            "onupdatetarget", gameObject,
            "onupdate", "UpdateColor"
        ));
    }
}
